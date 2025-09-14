<template>
	<v-container
		ref="tcwrapper"
		fluid
		class="tcwrapper"
		:class="{
			mobile: mobileView || selfMobileView,
			fs: fullScreenMode,
		}"
	>
		<div
			v-show="selectedImage && !fullScreenMode"
			class="masthead"
		>
			<v-img
				v-show="!mobileView && !selfMobileView"
				ref="masthead"
				:src="selectedImage"
				center
				alt="Welcome. Get ready for a new site experience."
			/>
			<v-img
				v-show="mobileView || selfMobileView"
				ref="masthead"
				:src="selectedImageMobile"
				center
				alt="Welcome. Get ready for a new site experience."
			/>
		</div>

		<v-container
			fluid
			class="tcwrapper-inner"
		>
			<div
				v-show="showForm"
				class="showForm"
			>
				<v-img
					v-show="!fullScreenMode"
					class="personalized"
					src="/img/personalized.png"
					width="570"
					center
					alt="Personalized Website Solution powered by CORE 2.0"
				/>
				<div
					v-show="!fullScreenMode"
					ref="subhead"
					class="subhead"
					v-html="subhead"
				></div>

				<v-container
					v-show="!submitted"
					class="formwrapper"
				>
					<h1 v-show="!fullScreenMode">
						Accept the Terms & Conditions
					</h1>
					<h2
						v-show="!fullScreenMode"
						ref="h2"
					>
						Fill out the following form to accept the new Terms &
						Conditions for our new platform.
					</h2>
					<v-form
						ref="form"
						v-model="valid"
						lazy-validation
					>
						<v-container
							v-show="!fullScreenMode"
							class="formfieldscontainer"
						>
							<v-row>
								<v-col class="formfields">
									<v-text-field
										id="firstname"
										v-model="firstname"
										label="First Name"
										flat
										outlined
										dense
										background-color="#fff"
									></v-text-field>
								</v-col>

								<v-col class="formfields">
									<v-text-field
										id="lastname"
										ref="lastname"
										v-model="lastname"
										label="Last Name"
										outlined
										dense
										background-color="#fff"
									></v-text-field>
								</v-col>

								<v-col class="formfields">
									<v-text-field
										id="marketerid"
										ref="id"
										v-model="id"
										:rules="idRules"
										label="Marketer ID*"
										outlined
										dense
										background-color="#fff"
										required
										maxlength="7"
									></v-text-field>
								</v-col>
							</v-row>
						</v-container>

						<v-btn
							v-show="
								(mobileView || selfMobileView) && fullScreenMode
							"
							ref="exitFullScreenMode"
							class="fullScreenMode exitFullScreenMode"
							plain
							@click="fullScreen(false)"
						>
							close <v-icon> mdi-arrow-collapse </v-icon>
						</v-btn>

						<div
							ref="tccontainerLabel"
							class="tccontainerLabel"
							:class="scrollError && errorClass"
						>
							Terms & Conditions
						</div>
						<div
							ref="tccontainer"
							class="tccontainer"
							:class="{
								'error--text': scrollError,
							}"
						>
							<div
								ref="tandc"
								v-scroll.self="onScroll"
								class="tandc v-virtual-scroll"
								required
							>
								<Terms />
							</div>
						</div>
						<v-text-field
							ref="tandchiddentinput"
							class="tandchiddentinput"
							:value="tandchiddentinputvalue"
							:rules="scrollRules"
							required
						>
						</v-text-field>

						<v-btn
							v-show="
								(mobileView || selfMobileView) && fullScreenMode
							"
							class="fullScreenMode exitFullScreenMode"
							plain
							@click="fullScreen(false)"
						>
							close <v-icon> mdi-arrow-collapse </v-icon>
						</v-btn>

						<div
							v-if="mobileView || selfMobileView"
							v-show="!fullScreenMode"
							class="mobileActions"
						>
							<v-btn
								class="fullScreenMode"
								plain
								@click="fullScreen(true)"
							>
								<v-icon> mdi-arrow-expand </v-icon> View
								fullscreen
							</v-btn>

							<!--<v-btn
								class="download"
								plain
								@click="generateTerms()"
							>
								<v-icon> mdi-tray-arrow-down </v-icon>

								Download
							</v-btn>-->
						</div>

						<v-checkbox
							v-show="!fullScreenMode"
							ref="agree"
							v-model="checkbox"
							class="agree"
							:rules="agreeRules"
							label="I agree to the Terms & Conditions*"
							required
						></v-checkbox>

						<v-container
							v-show="!fullScreenMode"
							class="buttoncontainer"
						>
							<v-btn
								ref="submit"
								color="success"
								x-large
								class="mr-4 submit"
								:class="formErrors && disabledClass"
								:ripple="!formErrors"
								@click="submit"
							>
								Submit
							</v-btn>
							<div class="">*Required</div>
						</v-container>
					</v-form>
				</v-container>

				<!--<VueHtml2pdf
					ref="html2Pdf"
					:show-layout="false"
					:float-layout="true"
					:enable-download="true"
					:preview-modal="false"
					:paginate-elements-by-height="1380"
					:pdf-quality="2"
					:manual-pagination="true"
					pdf-format="a4"
					pdf-orientation="portrait"
					pdf-content-width="700px"
					:html-to-pdf-options="htmlToPdfOptions"
				>
					<div slot="pdf-content">
						<Terms />
					</div>
				</VueHtml2pdf>-->

				<v-container
					v-show="submitted"
					ref="submittedwrapper"
					class="submittedwrapper"
				>
					<v-img
						src="/img/circle-check.svg"
						width="36px"
					/>

					<h1>
						You’ve successfully accepted the new Terms & Conditions
					</h1>
					<h2>You may now close this page.</h2>
				</v-container>
			</div>

			<Faq
				v-show="!fullScreenMode"
				class="faqcomponent"
				:mobile-view="mobileView"
			/>
		</v-container>
	</v-container>
</template>

<script>
export default {
	name: "TermsAndConditions",
	auth: false,
	components: {
		Faq: () => import("./faq.vue"),
		Terms: () => import("./terms.vue"),
	},
	layout: "blank",
	props: ["mobileView"],
	data: () => ({
		showForm: window.location.search.match(/faq/gi) ? false : true,
		faqurl: window.location.pathname.match(/responsiveform/gi)
			? "/library/content/responsiveform?v=faq"
			: "/library/content/form?v=faq",
		htmlToPdfOptions: {
			margin: 20,
			filename: "Terms & Conditions",
		},
		selfMobileView: false,
		fullScreenMode: false,
		selectedImage: "/img/termshero.png",
		selectedImageMobile: "/img/termsherom.png",
		errorClass: "error--text",
		disabledClass: "v-btn--disabled",
		formErrors: true,
		submitted: false,
		valid: true,
		firstname: "",
		lastname: "",
		id: "",
		scrollError: false,
		idRules: [
			(v) => !!v || "Marketer ID is required",
			(v) => (v && v.length == 7) || "Marketer ID must be 7 characters",
			(v) => /^\d{7}$/.test(v) || "Marketer ID must be numerical",
		],
		scrollRules: [
			(v) =>
				!!v ||
				"Please scroll down to read complete terms and conditions before accepting them.",
		],
		agreeRules: [(v) => !!v || "You must agree to continue"],
		checkbox: false,
		tandcscrolled: false,
		tandchiddentinputvalue: "",
		tccontainerHeight: null,
		subhead: `With our new, modern platform, you’ll have more tools at
					your disposal for customizing your page with an
					<b>easy-to-use page builder, custom templates</b>
					and <b>state-of-the-art integrations</b>.`,
	}),
	head: {
		title: "Welcome NYL Agents!",
	},
	computed: {
		target() {
			if (this.$refs.id.hasError) {
				if (this.mobileView || this.selfMobileView) {
					return this.$refs.lastname;
				}
				return this.$refs.h2;
			} else if (this.scrollError) {
				return this.$refs.tccontainerLabel;
			} else if (this.$refs.agree.hasError) {
				return this.$refs.agree;
			}
			return this.$refs.subhead;
		},
		options() {
			return {
				duration: 500,
			};
		},
	},
	watch: {
		valid() {
			this.checkAllRequirements();
		},
		checkbox() {
			this.checkAllRequirements();
		},
	},
	created() {
		this.updatePageWidth();
	},
	async mounted() {
		window.addEventListener("resize", this.updatePageWidth);
		window.addEventListener("scroll", this.windowScroll);
	},
	destroyed() {
		window.removeEventListener("resize", this.updatePageWidth);
		window.removeEventListener("scroll", this.windowScroll);
	},
	methods: {
		generateTerms() {
			// generate pdf for download
			this.$refs.html2Pdf.generatePdf();
		},
		checkAllRequirements() {
			if (this.$refs.id.valid && this.checkbox) {
				this.formErrors = false;
			} else {
				this.formErrors = true;
			}
		},
		windowScroll() {
			// check read thru of terms&conditions in fullscreen
			if (!this.fullScreenMode) return;
			var body = document.body,
				html = document.documentElement;
			var height = Math.max(
				body.scrollHeight,
				body.offsetHeight,
				html.clientHeight,
				html.scrollHeight,
				html.offsetHeight
			);
			if (window.scrollY + window.innerHeight >= height) {
				this.tandcscrolled = true;
				this.tandchiddentinputvalue = "true";
				this.scrollError = false;
			}
		},
		onScroll() {
			// check read thru of terms&conditions in non-fullscreen
			var maxScrollPosition =
				this.$refs.tandc.scrollHeight - this.$refs.tandc.clientHeight;
			if (this.$refs.tandc.scrollTop >= maxScrollPosition) {
				this.tandcscrolled = true;
				this.tandchiddentinputvalue = "true";
				this.scrollError = false;
			}
		},
		showFAQ(e) {
			e.preventDefault();
			if (this.submitted) {
				this.showForm = !this.submitted;
			}
		},
		async submit() {
			// validate/submit form
			try {
				console.log("here");
				if (this.$refs.form.validate()) {
					console.log("here");
					await this.$axios.post("/users/setacceptance", {
						firstName: this.firstname,
						lastName: this.lastname,
						marketerId: this.id,
						welcomePagePresented: true,
						acceptedTerms: true,
					});
					this.submitted = this.$refs.form.validate();
					this.scrollError = !this.tandcscrolled;
					this.onScroll();
					this.$vuetify.goTo(this.target, this.options);
				}
			} catch (err) {
				console.log(err);
			}
		},
		updatePageWidth() {
			// update browser resizing
			if (this.mobileView) return;
			this.$nextTick(() => {
				if (window.innerWidth <= 767) {
					this.selfMobileView = true;
				} else {
					this.selfMobileView = false;
				}
			});
		},
		fullScreen(open) {
			// open/close fullscreen
			this.fullScreenMode = open;
			if (this.fullScreenMode) {
				this.tccontainerHeight = this.$refs.tccontainer.offsetHeight;
				this.$refs.tccontainer.style.height = `auto`;
				setTimeout(() => {
					this.$vuetify.goTo(
						this.$refs.exitFullScreenMode,
						this.options
					);
				});
			} else {
				this.$refs.tccontainer.style.height = `${this.tccontainerHeight}px`;
				let cInt = setInterval(() => {
					if (this.$refs.id) {
						clearInterval(cInt);
						cInt = null;
						this.$vuetify.goTo(this.$refs.id, this.options);
					}
				}, 100);
			}
		},
		/* debugging methods
		reset() {
			this.$refs.form.reset();
			this.resetErrors();
		},
		resetValidation() {
			this.$refs.form.resetValidation();
			this.resetErrors();
		},
		resetErrors() {
			this.scrollError = false;
			this.tandcscrolled = false;
			this.tandchiddentinputvalue = "";
			this.formErrors = true;
		}
		*/
	},
};
</script>

<style lang="scss">
@media only screen and (max-width: 767px) {
	header,
	footer,
	.v-navigation-drawer {
		display: none !important;
	}

	.v-main {
		padding: 0 0 0 0 !important;
	}
}
.tcwrapper {
	max-width: 100%;
	padding: 0;

	.tcwrapper-inner {
		width: 1062px;
		max-width: 100%;
		padding: 0;
		margin: 0 auto;
	}

	.masthead {
		text-align: center;
	}

	.personalized {
		margin: 20px auto;
		display: flex;
	}

	.subhead {
		font-size: 24px;
		font-weight: 400;
		color: #243641;
		margin: 50px 100px;
		text-align: center;
	}

	.formwrapper {
		border-radius: 5px;
		background-color: #f8f8f8;
		padding: 50px 108px;
	}

	h1 {
		font-size: 42px;
		text-align: center;
		font-weight: 400;
		color: #0d3c59;
	}

	h2 {
		font-size: 18px;
		text-align: center;
		font-weight: 400;
		color: #243641;
	}

	.formfieldscontainer {
		padding: 0;
		margin: 40px 0 0 0;
	}

	.formfieldslabels {
		font-size: 14px;
		color: #243641;
	}

	.formfields label {
		font-size: 14px;
		color: #243641;
		top: -25px !important;
		left: -10px !important;
	}

	.formfields label.v-label--active {
		transform: none !important;
	}

	.formfields legend {
		display: none;
	}

	.formfields .v-text-field__details {
		padding: 0;
	}

	.tccontainerLabel {
		margin: 20px 0 10px 0;
		font-size: 20px;
		color: #002840;
	}

	.tccontainer {
		width: 100%;
		height: 300px;
		overflow: hidden;
		border: 1px solid #bdc5ca;
		border-radius: 5px;
		margin: 0 0 10px 0;
		padding: 15px 0px 5px 5px;
		background-color: #fff;
	}

	body::-webkit-scrollbar-thumb,
	.v-virtual-scroll::-webkit-scrollbar-thumb {
		min-height: 50px;
	}

	.tccontainerLabel.error--text {
		animation: v-shake 0.6s cubic-bezier(0.25, 0.8, 0.5, 1);
	}

	.tccontainer.error--text {
		border: 1px solid var(--v-error-base);
		outline: 1px solid var(--v-error-base);
	}

	.tandc {
		width: 100%;
		height: 100%;
		overflow: auto;
		padding-right: 20px;
		line-height: 30px;
		user-select: none;
		font-size: 14px;
		color: #243641;
	}

	.tandchiddentinput {
		padding: 0;
		margin: 0;
	}

	.tandchiddentinput .v-input__slot {
		display: none !important;
	}

	.agree {
		margin-top: 30px;
	}

	.agree label {
		font-size: 14px !important;
		color: #002840;
	}

	.buttoncontainer {
		display: flex;
		flex-direction: column;
		align-items: center;
	}

	.tcwrapper-inner .submit {
		margin: 20px 0 !important;
		padding: 16px 40px !important;
	}

	.tcwrapper-inner .submit.v-btn--disabled {
		box-shadow: none !important;
	}

	.tcwrapper-inner .success {
		background-color: #0079c2 !important;
	}

	.submittedwrapper {
		text-align: center;
		margin: 50px 0;
		border-radius: 5px;
		background-color: #f8f8f8;
		padding: 50px 106px;
		display: flex;
		justify-content: center;
		align-items: center;
		flex-direction: column;
	}

	.submittedwrapper h1 {
		font-weight: 500;
		font-size: 28px;
		color: #0d3c59;
		margin: 20px 0;
	}

	.submittedwrapper h2 {
		font-weight: 400;
		font-size: 18px;
		color: #243641;
		margin: 20px 0;
	}

	.submittedwrapper i {
		color: #0079c2 !important;
	}

	.faqcomponent {
		margin-top: 25px;
	}
	/*MOBILE STYILES*/
	&.mobile {
		position: relative;
		width: 100%;
		height: auto;
		overflow: auto;

		.tccontainer {
			padding: 15px 0px 5px 5px;
		}

		&.fs {
			/*Fullscreen mode*/
			position: relative;
			overflow: hidden;
			padding: 0;

			.formwrapper {
				background-color: #fff;
			}

			.tccontainer {
				border: none;
				padding: 15px 0px 5px 10px;
			}

			.tandc {
				height: auto;
			}

			.tccontainerLabel.error--text {
				color: #002840 !important;
			}

			.tccontainer.error--text {
				border: none !important;
				outline: none;
			}

			.tandchiddentinput {
				display: none;
			}

			.exitFullScreenMode {
				display: flex;
				align-items: center;
				margin: 0 0 0 auto;
				min-width: 40px;
			}

			.exitFullScreenMode i {
				margin: 0 0 0 5px;
			}
		}

		.tcwrapper-inner {
			width: 100%;
		}

		.subhead {
			font-size: 18px;
			line-height: 150%;
			margin: 20px;
		}

		h1 {
			width: 100%;
			margin-top: 20px;
			font-size: 34px;
			display: inline-block;
		}

		h2 {
			font-size: 18px;
			margin-bottom: 20px;
			display: inline-block;
		}

		.formwrapper {
			padding: 20px;
		}

		.formfieldscontainer .row {
			flex-direction: column;
		}

		.tccontainerLabel {
			margin-top: 20px;
		}

		.agree {
			margin-top: 10px;
		}

		.mobileActions {
			padding: 0;
			display: flex;
			justify-content: space-between;
		}

		.download,
		.fullScreenMode {
			padding: 0;
			position: relative;
			top: -5px;
		}

		.download:active,
		.fullScreenMode:active,
		.fullScreenMode:hover,
		.download:hover {
			background-color: transparent;
		}

		.fullScreenMode i,
		.download i {
			margin-right: 5px;
		}

		.tandchiddentinput {
			top: -5px;
			position: relative;
		}

		.submittedwrapper {
			padding: 20px;
			margin: 30px 0;
		}

		.submittedwrapper h1 {
			font-size: 24px;
		}

		.submittedwrapper h2 {
			font-size: 18px;
		}
	}
}
</style>
