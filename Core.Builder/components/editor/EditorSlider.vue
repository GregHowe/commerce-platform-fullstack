<template>
	<v-slider
		v-bind="$attrs"
		v-model="workingValue"
		class="slider"
		:append-icon="appendIcon"
		:prepend-icon="prependIcon"
		@click:prepend="prependClick"
		@click:append="appendClick"
	>
	</v-slider>
</template>

<script>
export default {
	name: "EditorSlider",
	inheritAttrs: false,
	props: {
		value: {
			type: Number,
			default: 0,
		},
		appendIcon: {
			type: String,
			default: "",
		},
		prependIcon: {
			type: String,
			default: "",
		},
		prependClick: {
			type: Function,
			default: () => {
				return true;
			},
		},
		appendClick: {
			type: Function,
			default: () => {
				return true;
			},
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
			return this.$emit("input", inputValue);
		},
	},
};
</script>
