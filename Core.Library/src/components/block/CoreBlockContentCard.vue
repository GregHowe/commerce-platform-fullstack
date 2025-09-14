<template>
	<div class="block__body block__content-card">
		<div class="container">
			<CoreBlock
				v-if="mediaSrc"
				:settings="{
					type: 'media',
					src: mediaSrc,
					height: 'auto',
					width: 'auto',
				}"
			/>

			<div class="block__section_text">
				<CoreBlock
					v-if="overlineText"
					:settings="{
						type: 'text',
						text: overlineText,
						tag: 'h4',
						variants: {
							size: 'overline',
						},
					}"
				/>
				<CoreBlock
					v-if="headlineText"
					:settings="{
						type: 'text',
						text: headlineText,
						tag: 'h5',
						variants: {
							size: 'headline',
						},
					}"
				/>
				<CoreBlock
					v-if="richText"
					:settings="{
						type: 'rich-text',
						richText,
					}"
				/>
				<div
					v-if="hasButtons"
					class="block__section_btns"
				>
					<CoreBlock
						v-if="hasPrimaryButton"
						:settings="{
							type: 'button',
							url: settings.primaryUrl,
							label: settings.primaryLabel,
							newTab: settings.primaryBtnNewTab,
							variants: { style: 'primary' },
						}"
					/>
					<CoreBlock
						v-if="hasSecondaryButton"
						:settings="{
							type: 'button',
							url: settings.secondaryUrl,
							label: settings.secondaryLabel,
							newTab: settings.secondaryBtnNewTab,
							icon: 'right-arrow',
							variants: {
								style: 'text',
								outline: true,
								'icon-placement': 'icon-right',
							},
						}"
					/>
				</div>

				<CoreBlock
					v-if="disclosureText"
					:settings="{
						type: 'rich-text',
						richText: disclosureText,
						variants: {
							size: 'disclosure',
						},
					}"
				/>
			</div>
		</div>
	</div>
</template>

<script>
import { renderData } from "@libraryHelpers/dataComponents.js";

export default {
	name: "CoreBlockContentCard",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		dataSite: {
			type: Object,
			required: true,
		},
	},
	computed: {
		overlineText() {
			return this.settings?.overline
				? renderData(this.settings?.overline, this.dataSite)
				: null;
		},
		headlineText() {
			return this.settings?.headline
				? renderData(this.settings?.headline, this.dataSite)
				: null;
		},
		richText() {
			return this.settings?.richText
				? renderData(this.settings?.richText, this.dataSite)
				: null;
		},
		disclosureText() {
			return this.settings?.disclosure
				? renderData(this.settings.disclosure, this.dataSite)
				: null;
		},
		mediaSrc() {
			return this.settings?.media || null;
		},
		hasButtons() {
			return this.hasPrimaryButton || this.hasSecondaryButton;
		},
		hasPrimaryButton() {
			return this.settings?.primaryButton || false;
		},
		hasSecondaryButton() {
			return this.settings?.secondaryButton || false;
		},
	},
};
</script>

<style lang="scss">
// .block.block--selected-deeply .block.block--selected {
//   outline: 0 solid transparent !important;
// }
.block_container_slim {
	.block_content-card {
		.container {
			border: 1px solid var(--_core__section_border-color);
		}
		.block_text_headline h5 {
			min-height: 100px;
		}
	}
}
.block_content-card {
	padding: 0 !important;
	align-self: flex-start;

	.block__body.block__content-card {
		padding-left: 0;
	}
	.container {
		padding: var(--_core__content_card-padding);
		// padding: 20px 20px;
		word-wrap: break-word;
	}
	.block_image .block__body .block__section_content img {
		object-fit: cover;
	}

	.block_text_title,
	.block_text_headline {
		// margin-top: 10px;
	}

	.block__body {
		display: flex;
		flex-direction: column;
		text-align: var(--_core__body_text-align);
		justify-content: var(--_core__body_justify-content);

		.block_text .block__body {
			color: var(--_core__content_card-font-color);
		}
		.block__section_text {
			display: flex;
			flex-direction: column;
			padding: var(--_core__content_card-text-padding);
			background-color: var(--_core__content_card-background-color);
			color: var(--_core__content_card-font-color);
			.block__body {
				color: var(--_core__content_card-font-color);
			}
			.block__section_btns {
				padding-bottom: 0;
			}
		}
		.block__section_btns {
			display: flex;
			flex-direction: row;
			align-items: center;
			justify-content: var(--_core__body_justify-content);
			padding-bottom: 1rem;
			width: 100%;
			& > .block {
				width: auto;
				margin: 0 0.2rem;
				&:first-child {
					margin-left: 0;
				}
				&:last-child {
					margin-right: 0;
				}
			}
		}
		.primary-button {
			a {
				border-style: solid;
				border-radius: 0.3rem;
				border-color: var(--_core__button_primary-border);
			}
		}
		.secondary-btn {
			a {
				background-color: var(--_core__button_secondary-border);
				border-style: solid;
				border-color: var(--_core__button_primary-border);
				color: var(--_core__button_secondary-color);
				border-radius: 0.3rem;
				inline-size: max-content;
			}
		}
		.block_button.block_button_text .button {
			color: var(--_core__content_card-link-color);
		}
		.block__section_overline {
			color: var(--_core__overline_color);
			font-size: var(--_core__overline_font-size);
			text-transform: var(--_core__overline_text-transform);
			font-family: var(--_core__overline_font-family);
			font-weight: 400;
			margin: 0.4rem 0;
		}
		.block__section_headline {
			font-size: var(--_core__header_font-size);
			line-height: var(--_core__header_line-height);
			color: var(--_core__header_color);
			text-transform: var(--_core__header_text-transform);
			font-family: var(--_core__header_font-family);
			padding-bottom: 1rem;
		}
		.block__section_disclosure {
			font-size: var(--_core__disclosure_font-size);
			color: var(--_core__disclosure_color);
			font-style: var(--_core__disclosure_font-style);
			line-height: var(--_core__disclosure_line-height);
		}
	}

	/* move these up to be variants available to every type of block */
	&.block_content-card_bg-color {
		--_core__container_background: var(--core__color_primary);
		--_core__container_color: var(--core__color_light-alt);
	}

	&.block_content-card_bg-dark {
		--_core__container_background: var(--core__color_dark);
		--_core__container_color: var(--core__color_light-alt);
	}

	&.block_content-card_bg-light {
		--_core__container_background: var(--core__color_light);
		--_core__container_color: var(--core__color_dark-alt);
	}

	&.block_content-card_image-right {
		@include breakpoint_up(lg) {
			.block__body {
				display: flex;
				flex-direction: row-reverse;
				align-items: center;
			}
		}
	}
	&.block_content-card_image-left {
		@include breakpoint_up(lg) {
			.block__body {
				display: flex;
				flex-direction: row;
				align-items: center;
			}
		}
	}
}
</style>
