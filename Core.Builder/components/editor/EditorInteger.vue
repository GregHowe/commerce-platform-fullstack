<template>
	<div class="editor editor-integer">
		<v-text-field
			v-bind="$attrs"
			v-model="workingValue"
			@blur="
				($event) => {
					workingValue = $event.target.value;
				}
			"
		/>
	</div>
</template>

<script>
export default {
	name: "EditorInteger",
	inheritAttrs: false,
	props: {
		value: {
			type: [Number, String],
			default: 0,
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
			let newValue = inputValue ? parseInt(inputValue) : 0;

			if (isNaN(newValue)) {
				return false;
			}
			this.$emit("input", newValue);
			return true;
		},
	},
};
</script>

<style></style>
