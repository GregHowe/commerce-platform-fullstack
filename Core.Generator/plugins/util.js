import {
	has as _has,
	get as _get,
	extend as _extend,
	isEqual as _isEqual,
	toLower as _toLower,
	set as _set,
	startCase as _startCase,
	cloneDeep as _cloneDeep,
} from "lodash";

export default ({ app }, inject) => {
	inject("util", {
		clone(obj) {
			return _cloneDeep(obj);
		},
		isEqual(value, other) {
			return _isEqual(value, other);
		},
		getPath(object, path, defaultValue = null) {
			return _get(object, path, defaultValue);
		},
		extend(object, value) {
			return _extend(object, value);
		},
		extendPath(object, path, value = null) {
			const existing = this.getPath(object, path);
			return this.setPath(object, path, _extend(existing, value));
		},
		hasPath(object, path) {
			return _has(object, path);
		},
		setPath(object, path, value = null) {
			_set(object, path, value);
			return object;
		},
		// returns the path to a property within an object or array
		// with a specified key and value
		findPath(object, key, value = null) {
			const path = [];
			const findKeyValue = (obj) => {
				if (!obj || (typeof obj !== "object" && !Array.isArray(obj))) {
					return false;
				} else if (
					Object.prototype.hasOwnProperty.call(obj, key) &&
					obj[key] === value
				) {
					return true;
				} else if (Array.isArray(obj)) {
					let parentKey = path.length ? path.pop() : "";
					for (let i = 0; i < obj.length; i++) {
						path.push(`${parentKey}[${i}]`);
						const result = findKeyValue(obj[i], key);
						if (result) {
							return result;
						}
						path.pop();
					}
				} else {
					for (const k in obj) {
						path.push(k);
						const result = findKeyValue(obj[k], key);
						if (result) {
							return result;
						}
						path.pop();
					}
				}
				return false;
			};
			findKeyValue(object);
			return path.length ? path.join(".") : null;
		},
		toTitleCase(str) {
			return _startCase(_toLower(str));
		},
		isValidJSON(str) {
			try {
				JSON.parse(str);
			} catch (e) {
				return false;
			}
			return true;
		},
	});
};
