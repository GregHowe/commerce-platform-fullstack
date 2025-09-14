<template>
	<div class="editor editor-color">
		<v-color-picker
			v-model="workingValue"
			v-bind="attrsOverride"
		/>
	</div>
</template>

<script>
import chroma from "chroma-js";
export default {
	name: "EditorColor",
	inheritAttrs: false,
	props: {
		value: {
			// it can accept objects as well,
			// but keeping it string only
			// might make our platform simpler
			type: [Object, String],
			default: null,
		},
	},
	data() {
		return {
			swatchDefaults: [
				["#000000FF"],
				["#444444FF"],
				["#888888FF"],
				["#BBBBBBFF"],
				["#FFFFFFFF"],
			],
		};
	},
	computed: {
		attrsOverride() {
			return {
				swatches: this.swatchDefaults,
				...this.$attrs,
				hideCanvas: true,
				hideInputs: true,
				hideModeSwitch: true,
				hideSliders: true,
				showSwatches: true,
			};
		},
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
			let newValue = inputValue?.hexa ? inputValue.hexa : inputValue;
			if (this.isValid(newValue) && this.isInSwatches(newValue)) {
				this.$emit("input", newValue);
				return true;
			}
		},
		isValid(hexa) {
			// it is a valid color value
			return chroma.valid(hexa);
		},
		isInSwatches(hexa) {
			// the chosen value is one of the default swatches
			return this.swatchDefaults.filter((val) => {
				return val[0] === hexa;
			}).length;
		},
	},
};
</script>

<style></style>
