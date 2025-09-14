<template>
	<div
		class="block__body"
		v-html="blockContent"
	/>
</template>

<script>
export default {
	name: "CoreBlockRichText",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		blockContent() {
			return this.settings?.richText || null;
		},
	},
};
</script>

<style lang="scss">
.block_rich-text {
	.block__body {
		font-size: var(--_core__container_font-size);
		text-transform: none;
		font-weight: 400;
		font-style: normal;
		color: var(--_core__container_color);
		font-family: var(--_core__container_font-family);
		text-align: var(--_core__body_text-align);
		margin: 0;
		& > p {
			margin: 0;
			+ p {
				margin-top: 0.8rem;
			}
		}
		& > ul {
			padding: 0 0 0 0.8rem;
			list-style: disc;
			margin: 1.2rem var(--_core__list_margin-right) 0.8rem
				var(--_core__list_margin-left);
			width: max-content;
			display: flex;
			flex-direction: column;
			text-align: left;
			& > li {
				list-style: disc;
				display: list-item;
				& > p {
					margin: 0;
				}
			}
		}
	}

	&.block_rich-text_footnote,
	&.block_rich-text_disclosure {
		.block__body {
			font-size: var(--_core__disclosure_font-size);
			color: var(--_core__disclosure_color);
			font-style: var(--_core__disclosure_font-style);
			line-height: var(--_core__disclosure_line-height);
		}
	}
}
</style>
