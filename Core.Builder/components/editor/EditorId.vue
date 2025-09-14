<template>
	<div class="editor editor-string">
		<ValidationProvider
			v-slot="{ errors }"
			ref="provider"
			:rules="rules"
			:name="providerName"
		>
			<v-text-field
				:value="localizedValue"
				dense
				v-bind="$attrs"
				:error-messages="errors"
				:label="label"
				outlined
				flat
				@blur="setLocalizedValue"
			/>
		</ValidationProvider>
	</div>
</template>

<script>
export default {
	name: "EditorString",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: "",
		},
		rules: {
			type: [String, Object],
			default: undefined,
		},
		providerName: {
			type: String,
			default: "name",
		},
		label: {
			type: String,
			default: null,
		},
	},
	computed: {
		localizedValue() {
			return this.value;
		},
	},
	methods: {
		emitInput(inputValue) {
			if (!this.$attrs.disabled && !this.$attrs.readonly)
				this.$emit("input", inputValue);
		},
		setLocalizedValue(e) {
			this.emitInput(e.target.value);
		},
	},
};
</script>
<style lang="scss">
.editor-string {
	margin-top: 0.2rem;
	& .v-label {
		font-size: 0.75rem;
	}
}
</style>
