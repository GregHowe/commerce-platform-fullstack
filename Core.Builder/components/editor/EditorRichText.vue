<template>
	<div v-if="editor">
		<label v-if="label">{{ label }}</label>
		<v-row class="my-4">
			<ForgeButton
				x-small
				text
				:class="{ 'v-item--active v-btn--active': isBold }"
				@click="editor.chain().focus().toggleBold().run()"
			>
				<v-icon
					small
					dense
				>
					mdi-format-bold
				</v-icon>
			</ForgeButton>

			<ForgeButton
				x-small
				text
				:class="{ 'v-item--active v-btn--active': isItalic }"
				@click="editor.chain().focus().toggleItalic().run()"
			>
				<v-icon small>mdi-format-italic</v-icon>
			</ForgeButton>
			<ForgeButton
				x-small
				text
				:class="{ 'v-item--active v-btn--active': isStrike }"
				@click="editor.chain().focus().toggleStrike().run()"
			>
				<v-icon small>mdi-format-strikethrough-variant</v-icon>
			</ForgeButton>
			<ForgeButton
				x-small
				text
				:class="{ 'v-item--active v-btn--active': isUnderline }"
				@click="editor.chain().focus().toggleUnderline().run()"
			>
				<v-icon small>mdi-format-underline</v-icon>
			</ForgeButton>
			<ForgeButton
				x-small
				text
				class="link-button"
				:class="{ 'v-item--active v-btn--active': isLink }"
				@click="toggleLinkEditor"
			>
				<v-icon small>mdi-link</v-icon>
			</ForgeButton>
			<ForgeButton
				text
				x-small
				:class="{
					'v-item--active v-btn--active': isBulletList,
				}"
				@click="editor.chain().focus().toggleBulletList().run()"
			>
				<v-icon small>mdi-format-list-bulleted</v-icon>
			</ForgeButton>
		</v-row>

		<EditorTiptap
			v-show="!isEditingLink"
			v-bind="$attrs"
			:editor="editor"
			class="tiptap-editor"
		/>

		<v-container v-show="isEditingLink">
			<EditorUrl
				v-bind="$attrs"
				:value="previousUrl"
				class="link-input"
				@input="
					($event) => {
						setLinkUrl = $event;
					}
				"
			>
			</EditorUrl>
			<v-row class="my-2">
				<ForgeButton
					text
					small
					outlined
					color="black"
					class="remove"
					@click="removeLink"
				>
					Remove
				</ForgeButton>
				<ForgeButton
					class="ml-auto save"
					text
					small
					outlined
					color="black"
					@click="setLink"
				>
					Save
				</ForgeButton>
			</v-row>
		</v-container>
	</div>
</template>

<script>
import { Editor, EditorContent as EditorTiptap } from "@tiptap/vue-2";
import StarterKit from "@tiptap/starter-kit";
import Link from "@tiptap/extension-link";
import Underline from "@tiptap/extension-underline";

export default {
	name: "EditorRichText",
	components: { EditorTiptap },
	inheritAttrs: false,
	props: {
		label: {
			type: String,
			default: null,
		},
		value: {
			type: String,
			default: "",
		},
	},
	data() {
		return {
			editor: null,
			isEditingLink: null,
			currentUrl: "",
			previousUrl: "",
		};
	},
	computed: {
		setLinkUrl: {
			get() {
				return this.value;
			},
			set(inputValue) {
				return (this.currentUrl = inputValue);
			},
		},
		isBold() {
			return this.editor.isActive("bold");
		},
		isItalic() {
			return this.editor.isActive("italic");
		},
		isStrike() {
			return this.editor.isActive("strike");
		},
		isUnderline() {
			return this.editor.isActive("underline");
		},
		isLink() {
			return this.editor.isActive("link");
		},
		isBulletList() {
			return this.editor.isActive("bulletList");
		},
	},
	mounted() {
		this.editor = new Editor({
			autofocus: true,
			content: this.value,
			extensions: [
				StarterKit,
				Link.configure({
					protocols: ["ftp", "mailto"],
					openOnClick: false,
					autolink: false,
				}),
				Underline,
			],
			onUpdate: () => {
				this.emitInput();
			},
		});
		this.$emit("editor", this.editor);
	},
	beforeDestroy() {
		this.editor.destroy();
		this.$emit("editor", null);
	},
	methods: {
		toggleLinkEditor() {
			this.isEditingLink = !this.isEditingLink;
			this.previousUrl = this.editor.getAttributes("link").href;
		},
		setLink() {
			if (this.currentUrl === "") {
				this.editor
					.chain()
					.focus()
					.extendMarkRange("link")
					.unsetLink()
					.run();
				this.isEditingLink = null;
				return;
			}

			this.editor
				.chain()
				.focus()
				.extendMarkRange("link")
				.setLink({ href: this.currentUrl })
				.run();
			this.isEditingLink = null;
		},
		removeLink() {
			this.editor.chain().focus().unsetLink().run();
			this.isEditingLink = null;
		},
		emitInput() {
			if (this.$attrs.disabled || this.$attrs.readonly) {
				return false;
			}
			const newValue = this.editor.getHTML();
			this.$emit("input", newValue);
			return true;
		},
	},
};
</script>

<style lang="scss">
.tiptap-editor {
	background-color: #f2f2f2;
	border-radius: 0.2rem;
	overflow: hidden;
	height: 200px;
	margin-bottom: 1.5rem;
	& .ProseMirror {
		padding: 0.2rem;
		overflow: auto;
		overflow-wrap: break-word;
		height: 100%;
		border-radius: 0.2rem;
	}
}
</style>
