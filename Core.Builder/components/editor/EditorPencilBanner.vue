<template>
	<div class="editor editor-pencil-banner">
		<label v-if="label">{{ label }}</label>
		<v-list-item>
			<EditorRichText
				v-model="bodyCopy"
				label="Body Copy"
				:rules="{ max: 255 }"
			/>
		</v-list-item>
		<v-list-item>
			<EditorString
				v-model="buttonText"
				label="Button Text"
				:rules="{ max: 50 }"
			/>
		</v-list-item>
		<v-list-item>
			<EditorUrl
				v-model="buttonLink"
				label="Link Button To"
			/>
		</v-list-item>
		<v-list-item>
			<EditorBoolean
				v-model="newTab"
				label="Open In New Tab"
			/>
		</v-list-item>
		<v-list-item>
			<EditorBoolean
				v-model="closeButton"
				label="Enable Close Button"
			/>
		</v-list-item>
	</div>
</template>

<script>
export default {
	name: "EditorPencilBanner",
	inheritAttrs: false,
	props: {
		value: {
			type: Object,
			default: () => {
				return {};
			},
		},
		fetchEditorMethod: {
			type: Function,
			required: true,
		},
		label: {
			type: String,
			default: null,
		},
	},
	computed: {
		bodyCopy: {
			get() {
				return this.value?.bodyCopy;
			},
			set(inputValue) {
				this.emitInput({ ...this.value, ...{ bodyCopy: inputValue } });
			},
		},
		buttonText: {
			get() {
				return this.value?.buttonText;
			},
			set(inputValue) {
				this.emitInput({
					...this.value,
					...{ buttonText: inputValue },
				});
			},
		},
		buttonLink: {
			get() {
				return this.value?.buttonLink;
			},
			set(inputValue) {
				this.emitInput({
					...this.value,
					...{ buttonLink: inputValue },
				});
			},
		},
		newTab: {
			get() {
				return this.value?.newTab;
			},
			set(inputValue) {
				this.emitInput({
					...this.value,
					...{ newTab: inputValue },
				});
			},
		},
		closeButton: {
			get() {
				return this.value?.closeButton;
			},
			set(inputValue) {
				this.emitInput({
					...this.value,
					...{ closeButton: inputValue },
				});
			},
		},
	},
	methods: {
		emitInput(inputValues) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}
			this.$emit("input", inputValues);
		},
	},
};
</script>
