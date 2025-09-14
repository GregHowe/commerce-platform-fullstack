<template>
	<div>
		<label>{{ label }}</label>
		<v-text-field
			v-model="localizedValue"
			v-bind="$attrs"
			type="time"
			role="time"
			step="2"
			outlined
			flat
		>
		</v-text-field>
	</div>
</template>

<script>
export default {
	name: "EditorTime",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: null,
		},
		label: {
			type: String,
			default: null,
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
