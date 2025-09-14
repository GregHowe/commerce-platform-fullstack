<template>
	<v-radio-group
		v-bind="$attrs"
		:value="workingValue"
		active-class
		@change="
			($event) => {
				workingValue = $event;
			}
		"
	>
		<v-radio
			v-for="item in items"
			:key="item.value"
			:color="item.color"
			:label="item.label"
			:name="item.value"
			:value="item.value"
			data-test="radio"
		>
		</v-radio>
	</v-radio-group>
</template>

<script>
export default {
	name: "EditorRadio",
	inheritAttrs: false,
	props: {
		items: {
			type: Array,
			required: true,
		},
		value: {
			type: String,
			default: "",
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
		emitInput(radioValues) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}

			this.$emit("input", radioValues);
			return true;
		},
	},
};
</script>

<style></style>
