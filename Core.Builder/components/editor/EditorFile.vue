<template>
	<div class="editor editor-file">
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
				ref="file"
				type="file"
				class="d-none"
				@change="updateFile"
			/>
			<v-card
				width="200"
				class="d-flex flex-column justify-space-around"
				flat
			>
				<v-btn
					color="primary"
					class="text--white"
					@click="selectFile"
					>{{ workingValue ? "Replace" : "Upload" }} File
				</v-btn>
				<v-btn
					v-if="workingValue"
					color="primary"
					class="text--white"
					@click="clearFile"
					>Clear File
				</v-btn>
				<v-btn
					v-if="workingValue"
					color="primary"
					class="text--white"
					@click="downloadFile"
					>Download File
				</v-btn>
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
import { url } from "~/../Core.Library/src/helpers/regex";

export default {
	name: "EditorFile",
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
			default: "File",
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
		selectFile() {
			this.$refs.file.click();
		},
		clearFile() {
			this.workingValue = null;
			this.$refs.provider.reset();
		},
		downloadFile() {
			if (this.workingValue) {
				const link = document.createElement("a");
				link.href = this.workingValue;
				const filename = this.workingValue.split("/").pop();
				link.setAttribute("download", filename);
				document.body.appendChild(link);
				link.click();
				document.body.removeChild(link);
			}
		},
		async updateFile(event) {
			const file = event.target.files[0];
			if (!file) {
				return;
			}
			const validation = await this.validateFile(file);
			if (!validation.valid) return;

			const uploadResult = await this.uploadFile({ file });
			this.workingValue = uploadResult;
		},
		async validateFile(file) {
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
