<template>
	<EditorString
		v-model="localizedValue"
		v-bind="$attrs"
		:rules="rules"
		:provider-name="providerName"
		hint="name@email.com"
		persistent-hint
	/>
</template>

<script>
export default {
	name: "EditorEmail",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: null,
		},
		providerName: {
			type: String,
			default: "Email",
		},
		rules: {
			type: [String, Object],
			default: "email",
		},
	},
	computed: {
		localizedValue: {
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
			if (!this.$attrs.disabled && !this.$attrs.readonly)
				this.$emit("input", inputValue);
		},
	},
};
</script>
