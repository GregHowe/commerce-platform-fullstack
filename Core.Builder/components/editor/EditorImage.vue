<template>
	<div class="editor editor-image">
		<!--need to disable eslint for this line so we can use scoped slot in script-->
		<!--eslint-disable vue/no-unused-vars-->
		<ValidationProvider
			v-slot="{ errors, validate }"
			ref="provider"
			:name="providerName"
			:rules="rules"
		>
			<!--eslint-enable vue/no-unused-vars-->
			<label v-if="label">{{ label }}</label>
			<input
				ref="image"
				type="file"
				class="d-none"
				@change="updateImage"
			/>
			<v-card
				width="200"
				class="d-flex flex-column justify-space-around"
				flat
			>
				<img
					v-if="workingValue"
					class="mb-2"
					:src="workingValue"
				/>
				<ForgeButton
					text
					class="mb-2 primary"
					color="white"
					@click="selectImage"
					>{{ workingValue ? "Replace" : "Upload" }} Image
				</ForgeButton>
				<ForgeButton
					v-if="workingValue"
					text
					class="mb-2 primary"
					color="white"
					@click="clearImage"
					>Clear Image
				</ForgeButton>
			</v-card>

			<div
				v-if="hint"
				class="v-messages theme--light mt-3"
			>
				<div class="v-messages__wrapper">
					<div class="v-messages__message">
						{{ hint }}
					</div>
				</div>
			</div>

			<div
				v-if="errors.length"
				class="v-messages theme--light error--text mt-1"
				role="alert"
			>
				<div class="v-messages__wrapper">
					<div
						v-for="(error, index) in errors"
						:key="index"
						class="v-messages__message"
					>
						{{ error }}
					</div>
				</div>
			</div>
		</ValidationProvider>
	</div>
</template>

<script>
import { mapActions } from "vuex";
import { url } from "~/../Core.Library/src/helpers/regex.js";

export default {
	name: "EditorImage",
	inheritAttrs: false,
	props: {
		value: {
			type: String,
			default: null,
		},
		label: {
			type: String,
			default: null,
		},
		hint: {
			type: String,
			default: null,
		},
		providerName: {
			type: String,
			default: "Image",
		},
		rules: {
			type: [String, Object],
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
	},
	methods: {
		...mapActions("content", ["uploadFile"]),
		selectImage() {
			this.$refs.image.click();
		},
		clearImage() {
			this.workingValue = null;
			this.$refs.provider.reset();
		},
		async updateImage(event) {
			const file = event.target.files[0];
			const validation = await this.validateImage(file);
			if (!validation.valid) return;
			this.workingValue = await this.uploadFile({ file });
		},
		async validateImage(file) {
			return await this.$refs.provider.validate(file);
		},
		emitInput(inputValue) {
			if (inputValue !== null && !url(inputValue)) {
				return false;
			}
			this.$emit("input", inputValue);
		},
	},
};
</script>
