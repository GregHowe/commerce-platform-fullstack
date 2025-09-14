<template>
	<EditorString
		v-model="workingValue"
		v-bind="$attrs"
		persistent-hint
		:hint="hint"
		:provider-name="providerName"
		:rules="rules"
		:label="label"
	/>
</template>

<script>
export default {
	name: "EditorUrl",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: "",
		},
		label: {
			type: String,
			default: "",
		},
		hint: {
			type: String,
			default: "https://{yourdomain}.com",
		},
		providerName: {
			type: String,
			default: "url",
		},
		rules: {
			type: [String, Object],
			default: "url",
		},
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
