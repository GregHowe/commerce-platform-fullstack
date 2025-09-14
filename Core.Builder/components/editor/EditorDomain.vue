<template>
	<EditorString
		v-bind="$attrs"
		v-model="workingValue"
		persistent-hint
		hint="Enter valid domain"
		:rules="rules"
		:provider-name="providerName"
	/>
</template>

<script>
export default {
	name: "EditorDomain",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: "",
		},
		providerName: {
			type: String,
			default: "Domain",
		},
		rules: {
			type: [String, Object],
			default: "domain",
		},
	},
	data() {
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
