<template>
	<div class="editor editor-select">
		<label class="v-label">{{ label }}</label>
		<div class="d-flex flex-grow-1">
			<v-select
				v-model="workingValue"
				class="text-body-2"
				v-bind="$attrs"
				solo
				dense
				:background-color="backgroundColor"
				append-icon="mdi-chevron-down"
				flat
			/>
		</div>
	</div>
</template>

<script>
export default {
	name: "EditorSelect",
	inheritAttrs: false,
	props: {
		value: {
			type: [String, Array, Number],
			default: null,
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
				this.emitInput(inputValue);
			},
		},
		backgroundColor() {
			return this.$attrs["background-color"] || "#f2f2f2";
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
