<template>
	<div class="editor editor-media">
		<label v-if="label">{{ label }}</label>
		<ForgeButtonGroup
			v-if="showButtonGroup"
			v-model="selectedType"
			class="mb-5"
			:buttons="buttons"
			:mandatory="true"
			dense
		/>
		<component
			:is="selectedComponent"
			:value="workingValue"
			:rules="rules"
			:provider-name="label"
			@input="workingValue = $event"
		/>
		<div
			v-if="hint"
			class="v-messages mt-3"
		>
			<div class="v-messages__wrapper">
				<div class="v-messages__message">
					{{ hint }}
				</div>
			</div>
		</div>
	</div>
</template>
<script>
import blockSchema from "~/schemas/block.json";
export default {
	name: "EditorMedia",
	props: {
		value: {
			type: String,
			default: null,
		},
		acceptTypes: {
			type: Array,
			default: () => ["image", "video", "file"],
		},
		hint: {
			type: String,
			default: null,
		},
		label: {
			type: String,
			default: null,
		},
	},
	data() {
		return {
			selectedType: null,
		};
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
		schema() {
			const type = blockSchema.types[this.selectedType];
			// for image / file / video, the data we want is in the "src" field
			return type
				? type.fields.find((field) => field.key === "src")
				: null;
		},
		rules() {
			return this.schema?.rules;
		},
		providerName() {
			return this.schema?.label;
		},
		buttons() {
			return this.acceptTypes.map((type) => {
				return { value: type, icon: `mdi-${type}` };
			});
		},
		selectedComponent() {
			switch (this.autoSelectedType) {
				case "image":
					return "EditorImage";
				case "file":
					return "EditorFile";
				case "video":
					return "EditorVideo";
			}
			return null;
		},
		showButtonGroup() {
			return !this.workingValue && this.acceptTypes.length > 1;
		},
		autoSelectedType() {
			if (this.isImage) {
				this.setSelectedType("image");
			}
			if (this.isFile) {
				this.setSelectedType("file");
			}
			if (this.isVideo) {
				this.setSelectedType("video");
			}
			if (!this.selectedType && this.acceptTypes.length === 1) {
				// if url is not matched, and only one acceptType is passed,
				// automatically set that editor
				this.setSelectedType(this.acceptTypes[0]);
			}
			return this.selectedType;
		},
		isImage() {
			if (this.workingValue && this.acceptTypes.includes("image")) {
				return (
					this.workingValue.endsWith(".png") ||
					this.workingValue.endsWith(".jpg") ||
					this.workingValue.endsWith(".jpeg") ||
					this.workingValue.endsWith(".webp") ||
					this.workingValue.endsWith(".gif") ||
					this.workingValue.endsWith(".svg")
				);
			}
			return false;
		},
		isFile() {
			if (this.workingValue && this.acceptTypes.includes("file")) {
				return (
					this.workingValue.endsWith(".csv") ||
					this.workingValue.endsWith(".pdf") ||
					this.workingValue.endsWith(".html") ||
					this.workingValue.endsWith(".txt")
				);
			}
			return false;
		},
		isVideo() {
			if (this.workingValue && this.acceptTypes.includes("video")) {
				return (
					this.value.match("www.youtube.com/embed") ||
					this.value.match("player.vimeo") ||
					this.value.match("players.brightcove")
				);
			}
			return false;
		},
	},
	methods: {
		setSelectedType(value) {
			this.selectedType = value;
		},
		emitInput(inputValue) {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}
			this.$emit("input", inputValue);
		},
	},
};
</script>
