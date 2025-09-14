<template>
	<div class="editor editor-boolean">
		<v-list-item-title v-if="label">{{ label }}</v-list-item-title>
		<v-switch
			v-bind="$attrs"
			v-model="workingValue"
			inset
			dense
			color="primary"
			background-color="transparent"
		/>
	</div>
</template>

<script>
export default {
	name: "EditorBoolean",
	inheritAttrs: false,
	props: {
		value: {
			type: [Boolean, String],
			default: false,
		},
		label: {
			type: String,
			default: null,
		},
	},
	computed: {
		workingValue: {
			get() {
				return this.value;
			},
			set(inputValue) {
				this.emitBool(inputValue);
			},
		},
	},
	methods: {
		emitBool(inputValue) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}
			let newValue = !!inputValue;
			this.$emit("input", newValue);
			return true;
		},
	},
};
</script>

<style lang="scss">
.editor-boolean {
	.v-input,
	.v-input__slot {
		margin: 0;
	}
}
</style>
