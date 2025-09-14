<template>
	<div class="editor editor-video">
		<label v-if="label">{{ label }}</label>
		<EditorUrl
			v-bind="$attrs"
			v-model="workingValue"
			:provider-name="providerName"
			:rules="rules"
			hint="url must be an embed link"
		/>
		<ForgeButton
			v-if="iconByUrl"
			class="mt-2"
			color="primary"
			:href="workingValue"
			target="_blank"
		>
			<v-icon class="mr-2">{{ iconByUrl }}</v-icon
			>Link
		</ForgeButton>
	</div>
</template>

<script>
import { mapGetters } from "vuex";

export default {
	name: "EditorVideo",
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
		providerName: {
			type: String,
			default: "Video",
		},
		rules: {
			type: [String, Object],
			default: "url",
		},
	},
	computed: {
		...mapGetters("validation", ["validationFailedItem"]),
		workingValue: {
			get() {
				return this.value;
			},
			set(inputValue) {
				this.emitInput(inputValue);
			},
		},
		validationFailed() {
			return this.validationFailedItem(this.providerName);
		},
		iconByUrl() {
			if (this.workingValue) {
				if (this.workingValue.match("youtube.com/embed")) {
					return "mdi-youtube";
				}
				if (this.workingValue.match("player.vimeo")) {
					return "mdi-vimeo";
				}
				if (this.workingValue.match("players.brightcove")) {
					return "mdi-video";
				}
				return null;
			}
			return null;
		},
	},
	methods: {
		emitInput(inputValue) {
			if (
				this.$attrs.disabled ||
				this.$attrs.readonly ||
				this.$nextTick(() => {
					this.validationFailed;
				})
			) {
				return false;
			}
			this.$emit("input", inputValue);
		},
	},
};
</script>
