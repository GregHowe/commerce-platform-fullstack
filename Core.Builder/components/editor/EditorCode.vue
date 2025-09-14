<template>
	<div
		ref="editorElement"
		class="editor editor-code"
		style="z-index: 99999"
	>
		<ForgeButton @click="open"> Edit {{ label }} </ForgeButton>
		<div
			v-show="isEditing"
			class="editor-code_ide"
		>
			<client-only>
				<VueAceEditor
					v-model="workingValue"
					width="100%"
					height="100%"
					@init="editorInit"
				/>
			</client-only>
		</div>
	</div>
</template>

<script>
let VueAceEditor;
if (process.client) {
	VueAceEditor = require("vue2-ace-editor");
}
import { mapMutations, mapState } from "vuex";
export default {
	name: "EditorCode",
	components: {
		VueAceEditor,
	},
	inheritAttrs: false,
	props: {
		value: {
			type: [String, Array, Object], // should only be string
			default: null,
		},
		label: {
			type: String,
			default: "Custom Code",
		},
		rules: {
			type: [String, Object],
			default: null,
		},
		type: {
			type: String,
			default: null,
		},
	},
	data() {
		return {
			isEditing: false,
			editor: null,
			editorOptions: {},
			workingValue: null,
		};
	},
	computed: {
		...mapState("interface", ["isSettingsMaximized"]),
		editorLanguage() {
			return this?.type || "json";
		},
	},
	watch: {
		// want to get rid of this
		// when things get minized
		// if this is open for some reason its gotta close
		// reason for the watcher is because
		// external forces can change this setting
		isSettingsMaximized(newVal) {
			if (!newVal) {
				this.close();
			}
		},
	},
	methods: {
		...mapMutations("interface", ["setMaximizeSettings"]),

		editorInit(editor) {
			// sets the editor instance
			this.editor = editor;
			this.editor.setTheme("ace/theme/chaos");
			require("brace/ext/language_tools"); //language extension prerequsite...
			require("brace/mode/html");
			require("brace/mode/javascript"); //language
			require("brace/mode/css");
			require("brace/mode/json");
			require("brace/theme/chrome");
			require("brace/snippets/javascript"); //snippet
		},

		close() {
			this.setMaximizeSettings(false);
			this.isEditing = false;
			if (this.editorLanguage === "json") {
				if (this.isValidJSON(this.workingValue)) {
					this.emitInput(JSON.parse(this.workingValue));
				} else {
					console.error(
						"The value was not valid JSON",
						this.workingValue
					);
				}
			} else {
				this.emitInput(this.workingValue);
			}
			this.workingValue = ""; // next time this opens it will import the code again
		},

		open() {
			this.setMaximizeSettings(true);
			this.isEditing = true;
			if (!this.value) {
				this.workingValue = "";
			} else {
				this.workingValue =
					typeof this.value === "string"
						? this.value
						: JSON.stringify(this.value, null, 2);
			}
			this.editor.getSession().setMode(`ace/mode/${this.editorLanguage}`);
		},

		toggle() {
			if (!this.editor) {
				this.open();
			} else {
				this.close();
			}
		},

		emitInput(inputValue) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}
			this.$emit("input", inputValue);
			return true;
		},

		isValidJSON(value) {
			try {
				JSON.parse(value);
			} catch (e) {
				return false;
			}
			return true;
		},
	},
};
</script>
<style lang="scss">
.editor-code_ide {
	position: fixed !important;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	z-index: 6;
	height: 100vh !important;
}
</style>
