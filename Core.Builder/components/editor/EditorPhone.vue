<template>
	<EditorString
		v-bind="$attrs"
		v-model="workingValue"
		hint="123-456-7890"
		persistent-hint
		maxlength="12"
		rules="phone"
		class="editor-string"
	/>
</template>

<script>
export default {
	name: "EditorPhone",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: "",
		},
	},
	data: () => {
		return {};
	},
	computed: {
		workingValue: {
			get() {
				return this.value;
			},
			set(inputValue) {
				this.emitInput(inputValue);
			},
		},
	},
	methods: {
		emitInput(inputValue) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}

			this.$emit("input", inputValue);
		},
	},
};
</script>

<style></style>
