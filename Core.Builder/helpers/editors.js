import { camelCase as _camelCase, startCase as _startCase } from "lodash";
export default {
	getEditorBySchema(schemaType) {
		if (schemaType) {
			switch (schemaType) {
				/*
          code
          editors
        */
				case "code":
				case "css":
				case "html":
				case "template":
				case "vue":
				case "javascript":
				case "js":
				case "json":
					return "EditorCode";

				/*
          other
          editors
        */
				case "content":
					return "EditorRichText";
				case "navigation":
					return "EditorMainNavigation";
				default:
					return `Editor${_startCase(_camelCase(schemaType)).replace(
						/ /g,
						""
					)}`;
			}
		}
	},
};
