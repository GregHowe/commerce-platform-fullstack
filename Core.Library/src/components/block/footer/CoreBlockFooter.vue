<template>
	<footer class="footer">
		<CoreBlockFooterDisclosures
			v-show="showFooter"
			:site="site"
			:is-mobile="isMobile"
		/>

		<CoreBlockFooterBanner
			v-show="showFooter"
			:site="site"
		/>
		<div class="footer__content-wrapper">
			<div class="footer__content">
				<div
					v-show="showFooter"
					class="footer__top"
				>
					<div class="navbar">
						<nav class="nav">
							<div class="nav-left">
								<figure class="brand">
									<a
										v-if="logoStyle !== 'none'"
										:href="logoLink"
										class="brand_link"
									>
										<img
											:class="logoClass"
											:src="logo"
											:alt="logoAltText"
										/>
									</a>
									<figcaption
										v-if="logoStyle !== 'large'"
										class="brand_text"
										:class="{
											'brand_text--desktop': !isMobile,
											'brand_text--mobile':
												isMobile &&
												logoStyle !== 'none',
										}"
									>
										<p
											v-if="goHeadquarters"
											class="brand_text__subtitle"
										>
											{{ goHeadquarters }} General Office
										</p>
										<h3
											v-if="hasDataName && showName"
											class="brand_text__title"
										>
											{{ name }}
										</h3>
										<p
											v-if="jobTitle"
											class="brand_text__subtitle"
										>
											{{ jobTitle }}
										</p>
									</figcaption>
								</figure>
							</div>
							<div class="nav-right">
								<div
									v-if="hasSocialLink"
									class="footer__top--social-links"
								>
									<p class="brand_text__subtitle">
										Connect on Social
									</p>
									<CoreBlock
										:settings="{
											type: 'social-links',
											facebook,
											instagram,
											linkedin,
											twitter,
											youtube,
											variants: {
												style: 'slim',
												align: 'align-right',
											},
										}"
									/>
								</div>
							</div>
						</nav>
					</div>
					<div
						v-if="showSignUpForm"
						class="footer__top--email-form"
					>
						<form @submit.prevent="submitForm">
							<div class="footer__top--email-form__text-input">
								<input
									v-model="emailForm.email"
									type="text"
								/>
								<button><CoreIcon icon="right-arrow" /></button>
							</div>
							<div
								v-if="hasEmailDisclosure"
								class="footer__top--email-form__checkbox-input"
							>
								<label
									for="emailForm"
									class="footer__top--email-form__checkbox-input-label"
									><input
										id="emailForm"
										type="checkbox"
										:value="emailForm.disclosureChecked" />
									<span
										class="footer__top--email-form__checkbox-input-text"
										v-html="signUpFormDisclosure"
									></span
								></label>
							</div>
						</form>
					</div>
				</div>

				<nav v-show="showFooter && navItems.length > 1">
					<ul
						:class="{
							'subnav--desktop': !isMobile,
							'subnav--mobile': isMobile,
						}"
					>
						<CoreBlockNavItem
							v-for="page1 in pageTree"
							:key="page1.linkText + page1.linkUrl"
							:site="site"
							:navitem="page1"
							class="navbar_item navbar_item--dropdown"
						/>
					</ul>
				</nav>

				<div v-show="showFooter">
					<div
						v-show="copyright"
						class="disclosures"
						:class="{ 'disclosures--mobile': isMobile }"
					>
						<span class="disclosures-text">
							<p v-html="copyright" />
						</span>
					</div>
					<div
						v-show="address"
						class="disclosures"
						:class="{ 'disclosures--mobile': isMobile }"
					>
						<span class="disclosures-text">
							<p v-html="address" />
						</span>
					</div>
				</div>
			</div>
		</div>
	</footer>
</template>

<script>
import { renderData } from "@libraryHelpers/dataComponents.js";
export default {
	name: "CoreBlockFooter",
	props: {
		site: {
			type: Object,
			default: () => {
				return {};
			},
		},
		// the builder preview is set to mobile view
		isBuilderMobile: {
			type: Boolean,
			default: false,
		},
	},
	data() {
		return {
			windowWidth: process.client ? window.innerWidth : null,
			emailForm: {
				email: null,
				disclosureChecked: false,
			},
			clonedSite: null,
		};
	},
	computed: {
		pageTree() {
			try {
				const pageTree = this?.site?.navigation?.links
					.filter((l) => !l.parent)
					.map((l) => ({
						...l,
						target: l.openInNewTab ? "_blank" : "_self",
					}));
				return pageTree;
			} catch (err) {
				return [];
			}
		},
		footer() {
			return this.site.footer || {};
		},
		address() {
			return renderData(this.footer.address, this.clonedSite);
		},
		copyright() {
			return renderData(this.footer.copyright, this.clonedSite);
		},
		showFooter() {
			return this.footer.showFooter || false;
		},
		logo() {
			return (
				renderData(this.footer.logo, this.clonedSite) ||
				"data:image/png;base64,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"
			);
		},
		logoStyle() {
			return this.footer.logoStyle;
		},
		logoAltText() {
			return this.footer.logoAltText || this.name;
		},
		logoLink() {
			return this.footer.logoLink || "/";
		},
		hasDataName() {
			const dataName =
				renderData(this.footer.nameFooter, this.clonedSite) || "";
			return !!dataName.trim(); // the default mainnav name has spaces between vars, so it will end up something like "  " even if there are no values
		},
		name() {
			return this.hasDataName
				? renderData(this.footer.nameFooter, this.clonedSite)
				: this.site.title;
		},
		showName() {
			if (!this.isManagingPartner && this.isGO) {
				return false;
			}
			return true;
		},
		isManagingPartner() {
			if (
				this.site.user?.ddcUserData?.marketerTitleTpDesc.toLowerCase() ===
				"managing partner"
			) {
				return true;
			}

			return false;
		},
		jobTitle() {
			if (this.isManagingPartner) {
				return "Managing Partner";
			} else
				return renderData(this.footer.jobTitleFooter, this.clonedSite);
		},
		isGO() {
			return this.site.user?.employeeType === "GO" ? true : false;
		},
		goHeadquarters() {
			return this.site.user?.ddcUserData?.orgUnitDesc;
		},
		logoClass() {
			return `brand_image brand_image--${this.logoStyle}`;
		},
		hasSocialLink() {
			return (
				this.facebook ||
				this.linkedin ||
				this.instagram ||
				this.twitter ||
				this.youtube
			);
		},
		facebook() {
			return renderData(this.footer.facebook, this.clonedSite);
		},
		linkedin() {
			return renderData(this.footer.linkedin, this.clonedSite);
		},
		instagram() {
			return this.footer.instagram;
		},
		twitter() {
			return renderData(this.footer.twitter, this.clonedSite);
		},
		youtube() {
			return this.footer.youtube;
		},
		showSignUpForm() {
			return this.footer.showSignUpForm;
		},
		signUpFormDisclosure() {
			return this.footer.signUpFormDisclosure;
		},
		hasEmailDisclosure() {
			return (
				this.signUpFormDisclosure &&
				this.signUpFormDisclosure !== "<p></p>"
			);
		},
		navItems() {
			return this?.site?.navigation?.links || [];
		},
		isMobile() {
			if (this.isBuilderMobile) {
				return true;
			}
			// breakpoint needs to correspond with Core.Builder/assets/scss/mixins.scss
			const breakpoint = 768;
			return this.windowWidth && this.windowWidth < breakpoint;
		},
	},
	mounted() {
		window.addEventListener("resize", this.onResize);
	},
	created() {
		this.addDataToSite();
	},
	beforeDestroy() {
		window.removeEventListener("resize", this.onResize);
	},
	methods: {
		onResize() {
			this.windowWidth = window.innerWidth;
		},
		submitForm() {
			//console.log(this.emailForm);
		},
		addDataToSite() {
			// here we can add various data points to use in the footer via mustache templates
			// needs to be done in created so that the computed data is available in the mustache templates
			const year = process.client ? new Date().getFullYear() : null;
			this.clonedSite = { year, ...this.site };
		},
	},
};
</script>

<style lang="scss">
.page .footer .navbar {
	padding: 0.5rem 0.5rem;
	width: 100%;
	border: 0;
}
.footer {
	/* borrowed a lot of styles from the top nav to get this out the door for the client to view */
	padding-top: 3.5rem;

	.disclosures {
		color: var(--core__footer_nav-font_color);
		font-family: var(--core__footer-font-family);
		padding: 1.5rem 0;

		a {
			color: var(--core__footer-disclosure-link_color);
			text-decoration: var(
				--core__footer-disclosure-link_text-decoration
			);
			font-family: var(--core__footer-font-family);
		}
		.block__body {
			font-size: var(--_core__disclosure_font-size);
		}
		&-text {
			text-align: left;
			font-size: 0.7rem;
		}
		&--mobile {
			padding: 0 30px;
		}
	}
	.footer__content-wrapper {
		background: var(--core__footer-background_color);
		.footer__content {
			max-width: var(--_core__container_max-width);
			margin: 0 auto;
		}
	}
	.block_accordion {
		max-width: var(--_core__container_max-width);
		border-top: var(--_core__container_border);
		padding-top: 12px;
		margin: 0 auto 24px;
		.block__control_accordion {
			color: var(--core__color_secondary);
		}
		.block__section_main {
			padding: 0 0.5rem;
			color: var(--core__color_quaternary);
		}
		.block_text {
			width: 50%;
			> div {
				font-size: 14px;
			}
		}
		&.disclosures--mobile {
			.block_text {
				width: 100%;
			}
		}
	}
	.block_pencil-banner .block__body-container {
		justify-content: center;
		gap: 12px;
	}
	.brand {
		padding: 0;
		.brand_link {
			flex-shrink: 0;
			padding: 0;
			text-decoration: none;
		}
		.brand_image {
			margin-right: 1rem;
			width: auto;
			object-fit: cover;
			max-height: none;
			&--small {
				height: 3rem;
			}
			&--both {
				height: 5rem;
			}
			&--only {
				height: 15rem;
				max-width: 15rem;
			}
		}
	}
	.brand_text {
		text-align: left;
		line-height: 1.1;
		color: var(--core__footer-font_color);
		&--mobile {
			display: none;
		}
		&__title {
			font-size: 1.4rem;
			font-family: var(--_core__header_font-family);
			margin: 0;
			font-weight: var(--_core__header_font-weight);
		}
		&__subtitle {
			font-size: 0.9rem;
			margin: 0.5rem 0 0;
			font-weight: normal;
			& + .block {
				margin-top: 0.5rem;
			}
		}
	}
	// &__nav {
	// 	list-style: none;
	// 	padding: 3rem 0;
	// 	margin: 0 auto;
	// 	display: flex;
	// 	flex-wrap: wrap;
	// 	& a {
	// 		color: var(--core__footer-font_color);
	// 		font-weight: bold;
	// 		margin: 0 1rem;
	// 		&:hover {
	// 			cursor: pointer;
	// 			text-decoration: underline;
	// 		}
	// 	}
	// }
	&__top {
		padding: 0;
		margin: 0 auto;
		padding-top: 3.5rem;
		padding-bottom: 3.5rem;
		border-bottom: var(--_core__container_border);
		border-bottom-color: var(--_core__container_footer-border-color);
		display: flex;
		flex-wrap: wrap;
		align-items: center;
		justify-content: flex-start;

		&--title {
			&-section {
				display: flex;
				flex-direction: column;
			}
		}
		&--social-links {
			.block__body {
				flex-direction: row;
			}
			&-title {
				margin-left: 1.7rem;
			}
		}
		&--email-form {
			display: flex;
			justify-content: flex-start;
			align-items: center;
			&__text-input {
				display: flex;
				& input[type="text"] {
					border-bottom: solid var(--_core__navbar_color) 1px;
				}
				& button {
					margin-left: 1rem;
				}
			}
			&__checkbox-input {
				margin-top: 0.8rem;
				&-label {
					line-height: 0.8rem;
					display: flex;
					align-items: flex-start;
					font-size: 0.7rem;
					color: var(--core__footer-font_color);
				}
				&-text {
					margin-left: 0.5rem;
					inline-size: 18rem;
					overflow-wrap: break-word;
				}
			}
		}
	}
	// .subnav__footer {
	// 	color: var(--core__footer-font_color);
	// 	display: flex;
	// 	& a {
	// 		color: var(--core__footer-font_color);
	// 		margin: 0;
	// 	}
	// 	& svg {
	// 		margin-left: 0;
	// 	}
	// 	&--desktop {
	// 		flex-direction: row;
	// 		justify-content: flex-start;
	// 		& li > a {
	// 			font-weight: normal;
	// 		}
	// 	}
	.subnav {
		&--mobile {
			border-top: 1px solid black;
			display: flex;
			flex-direction: column;
			ul {
				margin-top: 10px;
			}
			ul > li {
				border-top: 1px solid #bcbcbc;
			}

			.navbar_item {
				margin: 0;
				padding: 10px;
				font-size: 18px;
				&--dropdown {
					border-bottom: 1px solid black;
				}
				.navbar_nav {
					margin: 10px 0 0 0;
				}
			}
		}
	}

	.footer__content-wrapper nav ul {
		color: var(--core__footer_nav-font_color);
		font-family: var(--core__footer-font-family);
		display: flex;
		justify-content: space-between;
		list-style: none;
		padding: 0;
		margin: 75px 0;
		text-align: left;
		> li {
			list-style: none;
			a {
				color: var(--core__footer_nav-font_color);
			}
		}

		ul {
			display: flex;
			flex-direction: column;
			font-size: 14px;
			margin: 25px 0;
			li {
				margin: 15px 0;
			}
			a {
				color: var(--core__footer_nav-font_color);
			}
		}

		&--desktop {
			flex-direction: row;
			justify-content: flex-start;
			& li > a {
				font-weight: normal;
			}
		}
		&--mobile {
			padding: 0 0 3rem;
			flex-direction: column;
			& ul {
				padding-left: 0;
			}
			& a {
				font-family: var(--core__font_primary);
				text-decoration: none;
				:hover {
					text-decoration: underline;
				}
			}
			& li {
				display: flex;
				justify-content: space-between;
				list-style: none;
				font-size: 0.8rem;
				width: auto;
				border-bottom: 1px solid #bcbcbc;
				padding: 16px 30px 16px 48px;
			}
			& h3 {
				display: flex;
				justify-content: space-between;
				font-size: 1rem;
				border-bottom: 1px solid #bcbcbc;
				padding: 16px 30px 16px 24px;
			}
			& svg {
				margin-left: 20px;
				margin-bottom: -1px;
			}
		}
	}
}

// block pointer-events in the builder only
.v-main__wrap .footer {
	pointer-events: none;
}
</style>
