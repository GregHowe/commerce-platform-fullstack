<template>
	<div class="editor editor-string">
		<ValidationProvider
			v-slot="{ errors }"
			ref="provider"
			:rules="rules"
			:name="providerName"
		>
			<v-text-field
				:id="$attrs.id || `editorinput-${label}`"
				v-model="localizedValue"
				dense
				v-bind="$attrs"
				:error-messages="errors"
				:label="label"
				outlined
				flat
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
<style lang="scss">
.editor-string {
	margin-top: 0.2rem;
	& .v-label {
		font-size: 0.75rem;
	}
}
</style>
