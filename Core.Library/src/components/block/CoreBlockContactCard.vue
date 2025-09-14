<template>
	<div class="block__body">
		<div
			v-if="blockPhoto && !photoBackground"
			class="block__section_photo"
		>
			<CoreBlock
				:settings="{
					type: 'image',
					src: blockPhoto,
					height: 'auto',
					width: '100%',
				}"
			/>
			<div
				v-if="!photoTop"
				class="block__icon_group"
			>
				<CoreBlock
					:settings="{
						type: 'social-links',
						facebook: blockFacebook,
						instagram: blockInstagram,
						linkedin: blockLinkedin,
						twitter: blockTwitter,
						youtube: blockYoutube,
					}"
				/>
			</div>
		</div>
		<div
			class="block__section_info"
			:style="[
				photoBackground
					? {
							'background-image': `url(${blockPhoto})`,
							'background-position': 'center',
							'padding-top': '12rem',
					  }
					: '',
			]"
		>
			<CoreBlock
				v-if="blockLogo && !photoBackground"
				:settings="{
					type: 'image',
					src: blockLogo,
					height: 'auto',
					width: '350px',
				}"
			/>
			<CoreBlock
				:settings="{
					type: 'text',
					text: blockContactName,
					variants: {
						size: 'title',
						class: 'contact-name',
					},
				}"
			/>
			<CoreBlock
				:settings="{
					type: 'text',
					text: blockContactTitle,
					variants: {
						class: 'contact-title',
					},
				}"
			/>

			<CoreBlock
				v-if="blockContactDescription"
				:settings="{
					type: 'text',
					text: blockContactDescription,
				}"
			/>
			<div
				v-if="blockContactLandline"
				class="block__section_info_contact"
			>
				<CoreIcon
					icon="mobile-phone"
					class="icon"
				/>
				<a :href="`tel:${blockContactLandline}`">{{
					blockContactLandline
				}}</a>
			</div>
			<div
				v-if="blockContactMobile"
				class="block__section_info_contact"
			>
				<CoreIcon
					icon="phone"
					class="icon"
				/>
				<a :href="`tel:${blockContactMobile}`">{{
					blockContactMobile
				}}</a>
			</div>
			<div
				v-if="blockContactFax"
				class="block__section_info_contact"
			>
				<CoreIcon
					icon="fax"
					class="icon"
				/>
				<p>{{ blockContactFax }}</p>
			</div>
			<div
				v-if="blockContactEmail"
				class="block__section_info_contact"
			>
				<CoreIcon
					icon="email"
					class="icon"
				/>
				<a :href="`mailto:${blockContactEmail}`">{{
					blockContactEmail
				}}</a>
			</div>

			<div
				v-if="hasAddressSection"
				class="block__section_info_contact"
			>
				<CoreIcon
					icon="map"
					class="icon"
				/>
				<div class="block__section_info_address">
					<p>{{ blockContactAddressLine1 }}</p>

					<p v-if="blockContactAddressLine2">
						{{ blockContactAddressLine2 }}
					</p>

					<div class="block__section_info_contact">
						<p>
							{{ blockContactAddressCity }},
							{{ blockContactAddressState }}
							{{ blockContactAddressZip }}
						</p>
					</div>
				</div>
			</div>

			<CoreBlock
				v-if="licensesCalifornia.length"
				:settings="{
					type: 'text',
					text: `CA License: #${licensesCalifornia[0].licenseIdNumber}`,
					variants: {
						class: 'info_license',
					},
				}"
			/>

			<CoreBlock
				v-if="licensesArkansas.length"
				:settings="{
					type: 'text',
					text: `AR License: #${licensesArkansas[0].licenseIdNumber}`,
					variants: {
						class: 'info_license',
					},
				}"
			/>

			<div
				v-if="photoTop || photoBackground || noPhoto"
				class="block__icon_group"
			>
				<CoreBlock
					:settings="{
						type: 'social-links',
						facebook: blockFacebook,
						instagram: blockInstagram,
						linkedin: blockLinkedin,
						twitter: blockTwitter,
						youtube: blockYoutube,
						variants: {
							style: 'slim',
							align: alignContent,
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
	name: "CoreBlockContactCard",
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
	data() {
		return {
			photoLocation: "",
		};
	},
	computed: {
		noPhoto() {
			return !this.blockPhoto && !this.photoTop && !this.photoBackground;
		},
		blockPhoto() {
			return renderData(this.settings.photo, this.dataSite);
		},
		blockLogo() {
			return this.settings.logo || null;
		},
		blockContactName() {
			return renderData(this.settings.name, this.dataSite);
		},
		blockContactTitle() {
			return renderData(this.settings.title, this.dataSite);
		},
		blockContactDescription() {
			return this.settings.description;
		},
		blockContactLandline() {
			return renderData(this.settings.landline, this.dataSite);
		},
		blockContactMobile() {
			return renderData(this.settings.mobile, this.dataSite);
		},
		blockContactFax() {
			return renderData(this.settings.fax, this.dataSite);
		},
		blockContactEmail() {
			return renderData(this.settings.email, this.dataSite);
		},
		hasAddressSection() {
			return !!(
				this.blockContactAddressLine1 ||
				this.blockContactAddressCity ||
				this.blockContactAddressState ||
				this.blockContactAddressZip
			);
		},
		blockContactAddressLine1() {
			return renderData(this.settings.address?.line1, this.dataSite);
		},
		blockContactAddressLine2() {
			return renderData(this.settings.address?.line2, this.dataSite);
		},
		blockContactAddressCity() {
			return renderData(this.settings.address?.city, this.dataSite);
		},
		blockContactAddressState() {
			return renderData(this.settings.address?.state, this.dataSite);
		},
		blockContactAddressZip() {
			return renderData(this.settings.address?.zip, this.dataSite);
		},
		blockFacebook() {
			return renderData(this.settings.facebook, this.dataSite);
		},
		blockLinkedin() {
			return renderData(this.settings.linkedin, this.dataSite);
		},
		blockInstagram() {
			return renderData(this.settings.instagram, this.dataSite);
		},
		blockTwitter() {
			return renderData(this.settings.twitter, this.dataSite);
		},
		blockYoutube() {
			return this.settings.youtube || null;
		},
		photoPlacement() {
			return this.settings?.variants["photo-placement"] || null;
		},
		alignContent() {
			return this.settings?.variants["align-content"] || "align-center";
		},
		photoBackground() {
			return ["background"].includes(this.photoPlacement);
		},
		photoTop() {
			return ["top"].includes(this.photoPlacement);
		},
		licenses() {
			return this.dataSite?.user?.ddcUserData?.ddcLicenseData || [];
		},
		licensesCalifornia() {
			return this.licenses.filter((item) => {
				return item.stateCountyCode === "CA";
			});
		},
		licensesArkansas() {
			return this.licenses.filter((item) => {
				return item.stateCountyCode === "AR";
			});
		},
	},
};
</script>

<style lang="scss">
.block_contact-card {
	.block_text > .block__body {
		font-family: var(--core__font_secondary);
	}
	.block_text_contact-title {
		> .block__body {
			margin-top: -6px;
			text-align: center;
			text-transform: uppercase;
			font-size: 14px;
			font-family: var(--core__font_secondary);
			@include breakpoint_down(lg) {
				margin-top: -10px;
			}
		}
	}
	.block__body {
		background-color: var(--_core__contactcard_container_background);
		color: var(--_core__contactcard_font_color);
		display: flex;
		flex-direction: column;
		justify-content: var(--_core__body_justify-content);
		@include breakpoint_up(lg) {
			.block__body {
				flex-direction: column;
				column-count: 2;
				column-width: 50%;
				margin: 0 auto;
			}
		}

		.block__section_photo {
			display: flex;
			flex-direction: column;
			justify-content: var(--_core__body_justify-content);
		}
		.block__section_info {
			display: flex;
			flex-direction: column;
			gap: 4px;
			text-align: var(--_core__body_text-align);
			padding: 20px;
			p {
				padding: 0;
				margin: 0.25em 0;
			}
			.block__section_info_contact {
				align-items: center;
				margin-bottom: 5px;
				margin-top: 24px;
				word-break: break-word;
				~ .block__section_info_contact {
					margin-top: 0;
				}
				a {
					text-decoration: underline;
					color: var(--_core__contactcard_anchor);
					white-space: nowrap;
					overflow: hidden;
					text-overflow: ellipsis;
				}
				.block__section_info_address {
					.block__section_info_contact {
						margin-top: 0;
					}
				}
			}

			.block__icon_group {
				margin-top: 10px;
			}
		}
		.block_text_base,
		.block_text_title,
		.block_text {
			.block__body {
				background-color: transparent;
				text-align: center;
				font-family: var(--_core__body_font-family);
				color: var(--_core__contactcard_font-color);
			}
		}
		.block_text_title {
			.block__body {
				font-size: var(--_core__heading_font-size--h5);
			}
		}
		.block_text_base {
			.block__body {
				font-size: var(--_core__heading_font-size--h6);
			}
		}
		.block__section_info_contact {
			display: flex;
			justify-content: var(--_core__body_justify-content);
			justify-content: left;
			align-content: center;
			align-items: flex-start;
			font-size: var(--_core__nav-link_font-size);
			padding: 6px 0;
			text-align: left;
			.icon {
				margin-top: 0.2em;
				margin-right: 0.7em;
				min-width: 24px;
			}
		}
		.block_text_info_license {
			.block__body {
				font-weight: 700;
				text-align: left;
				font-size: 16px;
				margin: 10px 0 10px 5px;
			}

			~ .block_text_info_license .block__body {
				margin: 5px 0 5px 5px;
			}
		}

		.block__section_info_address {
			display: flex;
			flex-direction: column;
			p {
				margin-bottom: 0;
			}
			.block__section_info_contact {
				padding: 0px;
				p {
					margin: 0;
				}
			}
		}
		.block__icon_group {
			display: flex;
			flex-direction: row;
			justify-content: var(--_core__body_justify-content);
			text-align: var(--_core__body_text-align);
			padding: 0.8rem 0;
		}
		.block__link {
			display: flex;
			flex-direction: column;
			justify-content: var(--_core__body_justify-content);
			margin: 0 0.5rem;
			height: 100%;
			// color: var(--_core__container_color);
		}
	}

	&.block_contact-card_top {
		.block__body {
			flex-direction: column;
		}
	}
	&.block_contact-card_left {
		@include breakpoint_up(lg) {
			.block__body {
				flex-direction: row;
			}
		}
	}
	&.block_contact-card_right {
		@include breakpoint_up(lg) {
			.block__body {
				flex-direction: row-reverse;
			}
		}

		.block__section_info {
			padding-left: 0;
			padding-right: 2em;
		}
	}
	&.block_contact-card_background {
		.block__body {
			// color: var(--_core__container_color);
		}
		.block__section_info {
			background-color: var(--_core__container_background);
			text-align: var(--_core__body_text-align);
			line-height: 1em;
			padding-left: 2rem;
			padding-right: 2rem;
		}
		.block__section_info_contact {
			justify-content: var(--_core__body_justify-content);
		}
		.block__section_photo {
			display: none;
		}
	}
	&.block_contact-card_padding {
		@include breakpoint_up(lg) {
			.block__body {
				padding: 1em;
			}
		}
	}
	&.block_contact-card_no-padding {
		.block__body {
			padding: 0;
		}
	}
	&.block_contact-card_light {
		.block__section_info {
			background-color: var(--_core__container_background-light);
			.block__icon_group {
				.block_social-links {
					.block__body {
						a {
							color: var(--_core__contactcard_anchor);
						}
					}
				}
			}
		}
	}
	&.block_contact-card_dark {
		.block__section_info {
			background-color: var(--_core__container_background-dark);
			//color: var(--core__color_light);
			.block__icon_group {
				.block_social-links {
					.block__body {
						a {
							color: var(--core__color_light);
						}
					}
				}
			}
		}
	}
}
</style>
