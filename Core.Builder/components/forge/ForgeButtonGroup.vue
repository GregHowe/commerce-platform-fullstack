<template>
	<v-btn-toggle
		v-bind="$attrs"
		:value="workingValue"
		class="text-body-2"
		data-test="buttonGroup"
		@change="
			($event) => {
				workingValue = $event;
			}
		"
	>
		<ForgeButton
			v-for="button in buttons"
			:key="button.value"
			text
			:data-test="`button${button.value}`"
			:value="button.value"
			class="text-body-2 text-capitalize"
		>
			<v-icon v-if="button.icon">{{ button.icon }}</v-icon>
			{{ button.value }}
		</ForgeButton>
	</v-btn-toggle>
</template>

<script>
export default {
	name: "ButtonGroup",
	inheritAttrs: false,
	props: {
		buttons: {
			type: Array,
			required: true,
		},
		value: {
			type: [Array, String],
			default: null,
		},
	},
	computed: {
		workingValue: {
			get() {
				return this.value;
			},
			set(inputValues) {
				this.emitInput(inputValues);
			},
		},
	},
	methods: {
		emitInput(inputValues) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}

			this.$emit("input", inputValues);
			return true;
		},
	},
};
</script>
<style
	lang="scss"
	scoped
>
.i .v-icon {
	color: red !important;
}
</style>
