//package org.ow2.dsrg.jpmf.util;
package du1;

import java.lang.annotation.Documented;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.Iterator;
import java.util.NoSuchElementException;
import java.util.logging.Level;
import java.util.logging.Logger;


/**
 * Utility class for configuration and validation of configurable objects.
 * <p>
 * A configurable object is an object that exposes configurable properties. A
 * configurable property is a simple field annotated with the {@link Property}
 * annotation. The values of the properties(i.e. fields) are set using
 * introspection. There are two ways to configure the value of a configurable
 * field:
 * <p>
 * <ol>
 * <li>If the field type is class that provides either a string constructor
 *(i.e. a constructor with a single {@code String} parameter) or a static
 * factory method {@code valueOf(String)} returning an instance of the class,
 * then the value of the field is set to an instance of the field type created
 * from a string representation of the value.</li>
 * <li>If creating instances of the field type is more complex, the field value
 * can be set through a method annotated with the {@link Setter} annotation.
 * Such a method must have a setter signature, i.e. it must return {@code void}
 * and accept a single {@code String} parameter.</li>
 * </ol>
 * <p>
 * Configurable properties of an object can be set using the
 * {@link #set(Object, String, String) set()} method, while validation of object
 * properties can be achieved using the {@link #check(Object) check()} method.
 * The validation ensures that no field representing a mandatory configurable
 * property contains {@code null}.
 */
public class Configurator {

	public static Logger log = Logger.getAnonymousLogger();


	/* ***********************************************************************
	 * ANNOTATIONS
	 * ***********************************************************************/

	@Documented
	@Retention(RetentionPolicy.RUNTIME)
	@Target(ElementType.FIELD)
	public @interface Property {

		/**
		 * Returns the name of the configuration property held in the annotated
		 * field.
		 *
		 * @return
		 *	  name of the property held in the annotated field, or empty string
		 *	  indicating that the name is identical to the field name
		 */
		String name() default "";

		/**
		 * Determines whether the property is required or not.
		 *
		 * @return
		 *	  {@code true} if the property must have a non-null value,
		 *	  {@code false} otherwise
		 */
		boolean isRequired() default true;

		/**
		 * Returns the string representation of the default value of the
		 * annotated field. Empty string indicates that the default value
		 * has not been set.
		 *
		 * @return
		 *	  string representation of the default value
		 */
		String defaultValue() default "";

		/**
		 * Returns an optional description of the property held in the
		 * annotated field.
		 *
		 * @return
		 *	  name
		 */
		String description() default "";
	}


	@Documented
	@Retention(RetentionPolicy.RUNTIME)
	@Target(ElementType.METHOD)
	public @interface Setter {

		/**
		 * Returns the name of the property configured by the annotated method.
		 *
		 * @return
		 *	  name of the property configured by the annotated method, or
		 *	  empty string indicating that the name corresponds to method
		 *	  name without the "set" prefix with the first letter in lower case
		 */
		String name() default "";

	}


	/* ***********************************************************************
	 * PUBLIC METHODS
	 * ***********************************************************************/

	/**
	 * Sets the given property to the given value in the given configurable
	 * object.
	 *
	 * @param target
	 *	  target configurable object to set the property on
	 * @param propertyName
	 *	  property name
	 * @param propertyValue
	 *	  property value
	 * @throws ConfigurationException
	 *	  if the value of the given property cannot be set on the
	 *	  given object
	 */
	public static void set(Object target, String propertyName, String propertyValue) {
		//
		// First try to obtain a method based setter, which is necessary
		// for more complex properties. If that fails, try to obtain a
		// field based setter, assuming the property is simple enough to
		// convert to object instance. If that also fails, log a warning.
		//
		PropertySetter setter = makeMethodSetter(target, propertyName);
		if (setter == null) {
			setter = makeFieldPropertySetter(propertyName, target);
		}
		if (setter == null) {
			if (log.isLoggable(Level.WARNING)) {
				log.log(Level.WARNING, "Unable to find configuration method for property %s", propertyName);
			}
			return;
		}

		//
		// Set the property value.
		//
		setter.setValue(propertyValue);
	}

	/**
	 * Returns the name of specified Property or it's field name, if property has empty name.
	 *
	 * @param property
	 *	  object with field field
	 * @param field
	 *	  field
	 */	
	protected static String getPropertyName(Property property, Field field) {
		assert(property != null && field != null);
		return ((property.name().length() > 0) ? property.name() : field.getName());
	}
	
	/**
	 * Returns the value of the given Field on the given object.
	 *
	 * @param target
	 *	  object with field
	 * @param field
	 *	  field
	 * @throws IllegalAccessException 
	 * @throws IllegalArgumentException 
	 */	
	protected static Object getObjectFieldValue(Object target, Field field) throws IllegalArgumentException, IllegalAccessException {
		if (field == null) {
			return null;
			// throw new NullPointerException("Required parametr field is null.");
		}
		//
		// Make the field accessible before getting its value and
		// restore the previous accessibility state after that.
		//
		boolean fieldAccesible = field.isAccessible();
		field.setAccessible(true);
		Object fieldValue = field.get(target);
		field.setAccessible(fieldAccesible);
		
		return fieldValue;
	}

	/**
	 * Sets the value of the given field on the given object to its default value.
	 *
	 * @param target
	 *	  object with field to set
	 * @param property
	 *	  property with its default value
	 * @param field
	 *	  field to set
	 * @throws ConfigurationException 
	 */	
	private static void configureFieldPropertyToDefault(Object target, Property property, Field field) throws ConfigurationException {
		//
		// Set default value for null fields.
		//
		String propertyName = getPropertyName(property, field);
		String defaultValue = property.defaultValue();
		if (defaultValue == "" && property.isRequired()) {
			throw new ConfigurationException("Required property '%s' is not configured", propertyName);
		}
		trace("setting field property %s to default value %s", propertyName, defaultValue);
		configureFieldProperty(propertyName, target, defaultValue, field);
	}
	
	/**
	 * Checks if all configurable fields in the passed object are not null.
	 *
	 * @param target
	 *	  object with configurable fields
	 * @throws ConfigurationException
	 */
	public static void check(Object target) throws ConfigurationException {
		//
		// Find all configurable fields and make sure that all mandatory
		// fields have a non-null value. If any configurable field with
		// a null value is found and has a default value specified, an
		// instance of the value is created and the field is initialized
		// to the default value.
		//
		// If any mandatory field remains uninitialized, throw an exception.
		//

		try {
			for (Field field : new AllDeclaredFieldsIterable(target.getClass())) {
				//
				// Skip fields without the @Property annotation or fields
				// with non-null value.
				//
				Property property = field.getAnnotation(Property.class);
				Object propertyValue = getObjectFieldValue(target, field);
				if (property == null || propertyValue != null) {
					continue;
				}

				configureFieldPropertyToDefault(target, property, field);
			}

		} catch(ConfigurationException configException) {
			// propagate without wrapping
			throw configException;
		} catch(Exception exception) {
			wrap(exception, "Unable to verify object property configuration!");
		}
	}


	/* ***********************************************************************
	 * PropertySetter
	 * ***********************************************************************/

	/**
	 * Private interface encapsulating a method for setting a property value on
	 * an object. Instances of the {@link PropertySetter} interface are bound to
	 * a particular target object and property name at construction time -- only
	 * the value remains unbound and can be set.
	 */
	private interface PropertySetter {
		void setValue(String value);
	}


	/* ***********************************************************************
	 * Field-based property setter
	 * ***********************************************************************/

	/**
	 * Returns a field based {@link PropertySetter} bound to the given object
	 * and property name. When setting the property value, the returned
	 * {@link PropertySetter} will modify the value of an object field annotated
	 * by the {@code @Property} annotation with matching name.
	 *
	 * @param propertyName
	 *	  name of the property to set
	 * @param target
	 *	  target object on which to set the property
	 *
	 * @return
	 *	  {@link PropertySetter} which allows to configure the property on
	 *	  the given object, or {@code null} if the target object has no field
	 *	  with matching annotation
	 */
	static PropertySetter makeFieldPropertySetter(final String propertyName, final Object target) {
		//
		// Find a configurable field for the given property and create a
		// PropertySetter for the property.
		//
		// Specifically, enumerate all fields in the given object's class
		// hierarchy and find the first field annotated with the
		// @Property annotation matching the given property field.
		//
		for (final Field field : new AllDeclaredFieldsIterable(target.getClass())) {
			String classPropertyName = null;
			Property property = field.getAnnotation(Property.class);
			if (property != null) {
				classPropertyName = getPropertyName(property, field);
			}

			if (propertyName.equals(classPropertyName)) {
				//
				// Match found -- create the setter.
				//
				return new PropertySetter() {
					public void setValue(String newValue) throws ConfigurationException {
						trace("setting field property %s to %s", propertyName, newValue);
						configureFieldProperty(propertyName, target, newValue, field);
					}
				};
			}
		}

		//
		// No match found.
		//
		return null;
	}


	/**
	 * Sets the given field of the given object to the given value. The
	 * value is converted from string representation to an instance of
	 * the field type.
	 *
	 * @param propertyName
	 *	  name of the property being configured
	 * @param target
	 *	  target object on which to set the field value
	 * @param propertyValue
	 *	  string value of the property being configured
	 * @param field
	 *	  the field to set to the given value
	 *
	 * @throws ConfigurationException
	 *	  if the string representation of the value could not be
	 *	  converted to an instance of the field type or if setting
	 *	  the field failed
	 */
	static void configureFieldProperty(String propertyName, Object target, String propertyValue, Field field) {
		//
		// Create an instance of the property value and set the field
		// value on the target object.
		//
		Object val = makeValueInstance(field, propertyValue);
		if (val == null) {
			throw new ConfigurationException("property %s: could not create %s instance for %s", propertyName, field.getType().getName(), propertyValue);
		}

		try {
			boolean oa = field.isAccessible();

			//
			// Make the field accessible before setting its value and
			// restore the previous accessibility state after that.
			//
			field.setAccessible(true);
			field.set(target, val);
			field.setAccessible(oa);
		} catch(Exception e) {
			wrap(e, "Unable to configure field %s with property %s=%s", field.getName(), propertyName, propertyValue);
		}
	}


	/**
	 * Creates an object instance from the string representation of a property value.
	 * The instance type is determined by the type of the given field and the instance
	 * is created either by calling a string constructor or a static factory method on
	 * the field class.
	 *
	 * @param field
	 *	  field to create value for
	 * @param fieldValue
	 *	  string representation of field value
	 * @return
	 *	  an object of the given field type representing the given value, or
	 *	  {@code null} if the instance could not be created
	 */
	static Object makeValueInstance(Field field, String fieldValue) {
		// First try to create the value instance by invoking a string constructor of the field class.
		try {
			return((Constructor<?>)((Class<?>) field.getType()).getConstructor(new Class<?>[] { String.class })).newInstance(fieldValue);
		} catch(Exception e) { /* quell the exception and try the next method */ }

		// If there is no suitable constructor, try to create the instance by invoking a static factory method.
		try {
			Method fact = ((Class<?>) field.getType()).getMethod("valueOf", new Class<?>[] { String.class });
			if (((Class<?>)field.getType()).isAssignableFrom(fact.getReturnType())) {
				return fact.invoke(null, fieldValue);
			}
		} catch(Exception e) { /* quell the exception */ }

		// Could not create the instance, return null.
		return null;
	}


	/* ***********************************************************************
	 * Method-based property setter
	 * ***********************************************************************/

	/**
	 * Returns a method based {@link PropertySetter} bound to the given object
	 * and property name. When setting the property value, the returned
	 * {@link PropertySetter} will invoke a setter method annotated by the
	 * {@link Setter} annotation with matching name.
	 *
	 * @param target
	 *	  target object on which to set the property
	 * @param propertyName
	 *	  name of the property to set
	 * @return
	 *	  {@link PropertySetter} which allows to configure the property on
	 *	  the given object, or {@code null} if the target object has no setter
	 *	  method with matching annotation
	 */
	static PropertySetter makeMethodSetter(final Object target, final String propertyName) {
		Class<?> tc;

		//
		// Find a setter method for the given property and create a
		// PropertySetter for the property.
		//
		// Specifically, enumerate all methods in the given object's class
		// hierarchy and find the first setter method annotated with the
		// @Setter annotation matching the given property field.
		//
		tc = target.getClass();
		do {
		for (final Method dm : tc.getDeclaredMethods()) {
			Setter str;
			String s;
			str = dm.getAnnotation(Setter.class);
			if (str == null) {
				s = null;
			}
			else {
			s = str.name();
			if (s.length() == 0) {
				//
				// If the method name starts with "set", strip the prefix and lower case the first letter of the suffix.
				//
				s = dm.getName();
				if (s.startsWith("set")) {
					s = s.substring(3, 4).toLowerCase() + s.substring(4);
				}
			}
			}
			if (propertyName.equals(s)) {
				//
				// Match found -- create the setter.
				//
				return new PropertySetter() {
				public void setValue(String v) {
				boolean oa;
				trace("setting method property %s to %s", propertyName, v);
				if ((Class<?>)dm.getReturnType() != void.class||dm.getParameterTypes()[0] != String.class||dm.getParameterTypes().length != 1) {
					throw new ConfigurationException("property %s: method %s() is not a setter", propertyName, dm.getName());
				}
					try {
						oa = dm.isAccessible();
						dm.setAccessible(true);
						dm.invoke(target, v);
						dm.setAccessible(oa);
					} catch(Exception e) {
					wrap(e, "Unable to set property %s=%s using method %s()", propertyName, v, dm.getName());
					}
				}
			};
			}
		}
		tc = tc.getSuperclass();
		} while (tc != null);
		//
		// No match found.
		//
		return null;
	}


	/* ***********************************************************************
	 * Logging
	 * ***********************************************************************/

	private static void trace(String format, Object ... args) {
		if (log.isLoggable(Level.FINE)) {
			log.log(Level.FINE, format, args);
		}
	}


	/* ***********************************************************************
	 * ConfigurationException
	 * ***********************************************************************/

	/**
	 * Common exception for all configuration errors.
	 */
	public static class ConfigurationException extends RuntimeException {
		ConfigurationException(String format, Object ... args) {
			super(String.format(format, args));
		}
		ConfigurationException(Throwable throwable, String format, Object ... args) {
			super(String.format(format, args), throwable);
		}
	}


	/**
	 * Wraps the given {@link Throwable} as a {@link ConfigurationException}
	 * along with an additional formatted message.
	 */
	private static void wrap(Throwable throwable, String format, Object ... args) {
		throw new ConfigurationException(throwable, format, args);
	}



	/* ***********************************************************************
	 * AllDeclaredFieldsIterable
	 * ***********************************************************************/

	/**
	 * Provides an iterable interface to fields declared in a class hierarchy.
	 * The provided iterator traverses the inheritance hierarchy from the given
	 *(leaf) class to the(root) base class and returns all fields declared in
	 * the classes along the inheritance path. The iterator(obviously) does not
	 * support element removal, since it operates on an immutable structure.
	 */
	static class AllDeclaredFieldsIterable implements Iterable<Field> {
		private Class<?> leaf;

		/**
		 * Creates an iterable for the given leaf class. If the leaf class
		 * is {@code null}, the iterable produces an empty iterator.
		 */
		AllDeclaredFieldsIterable(Class<?> leafClass) {
			leaf = leafClass;
		}

		@Override
		public Iterator<Field> iterator() {
			return new Iterator<Field>() {

			private Class<?> klass = leaf;
			private Iterator<Field> fields = new ArrayIterator<>(new Field[0]);

			public boolean hasNext() {
				//
				// If there are no fields, check if there is another class
				// where to look for fields. If we run out of classes, there
				// are no more fields left.
				//
				while (!fields.hasNext()) {
					if (klass == null) {
						return false;
					}
					fields = new ArrayIterator<Field>(klass.getDeclaredFields());
					klass = klass.getSuperclass();
				}
				return true;
			}

			@Override
			public Field next() {
				if (!hasNext()) {
					throw new NoSuchElementException();
				}
				return fields.next();
			}

			public void remove() {
				throw new UnsupportedOperationException();
			}
		};
		}
	}


	/* *******************************************************************
	 * ArrayIterator
	 * *******************************************************************/

	static class ArrayIterator<E> implements Iterator<E> {
		private E[] array;
		private int length;
		private int position;

		ArrayIterator(E[] array) {
			this.array = array;
			length = array.length;
			position = 0;
		}

		@Override
		public void remove() {
			throw new UnsupportedOperationException("cannot remove elements from array");
		}

		public boolean hasNext() {
			return position < length;
		}

		@Override
		public E next() {
			if (!hasNext()) {
				throw new NoSuchElementException();
			}
			return array[position++];
		}
	}

}
