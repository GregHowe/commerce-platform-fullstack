<template>
	<v-card
		v-bind="$attrs"
		flat
		class="white mr-2"
	>
		<v-card-title>
			<v-row class="align-center justify-center">
				<h5 class="font-weight-bold text-body-1">Icons</h5>

				<ForgeButton
					v-if="!$attrs.mandatory"
					class="ml-auto remove-btn"
					text
					x-small
					@click="workingValue = null"
					>Remove
				</ForgeButton>
			</v-row>
		</v-card-title>

		<v-card-actions
			class="dense"
			color="white"
		>
			<v-btn-toggle
				:value="workingValue"
				dense
				borderless
				class="flex-wrap"
				background-color="white"
				@change="
					($event) => {
						workingValue = $event;
					}
				"
			>
				<v-btn
					v-for="icon in schema"
					:key="icon.value"
					:value="icon.value"
					small
					icon
					:class="icon.value"
				>
					<CoreIcon
						class="mx-2 text--primary"
						:icon="icon.value"
					/>
				</v-btn>
			</v-btn-toggle>
		</v-card-actions>
	</v-card>
</template>

<script>
import iconSchema from "~/../Core.Library/src/schemas/icon.json";
export default {
	name: "EditorIcon",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: "",
		},
	},
	computed: {
		schema() {
			return iconSchema;
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
			this.$emit("input", inputValue);
			return true;
		},
	},
};
</script>

<style lang="scss"></style>
