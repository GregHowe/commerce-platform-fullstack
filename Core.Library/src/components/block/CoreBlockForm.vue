<template>
	<div class="block__body">
		<div class="container">
			<div
				v-if="status === 'pending'"
				class="form--pending"
			>
				<p>Submitting form, one moment...</p>
				<progress></progress>
			</div>

			<div
				v-else-if="status === 'success'"
				class="form--success"
			>
				<CoreIconCircleCheck />
				<p>
					{{ onSuccessMessage }}
				</p>
			</div>

			<div
				v-else-if="status === 'fail'"
				class="form--fail"
			>
				<CoreIconError class="fail-icon" />
				<p>There was problem submitting your form.</p>
			</div>

			<div
				v-else
				class="form"
			>
				<ValidationObserver
					v-slot="{ handleSubmit }"
					:slim="true"
				>
					<form @submit.prevent="handleSubmit(submit)">
						<!-- first and last name -->
						<div class="row">
							<ValidationProvider
								v-slot="{ errors }"
								:rules="{
									max: 31,
									required: true,
								}"
								class="col form-field"
								name="First Name"
								tag="div"
							>
								<label
									for="first-name"
									class="required"
								>
									First Name
								</label>
								<input
									id="first-name"
									v-model.trim="form.firstName"
									type="text"
									name="first-name"
									required
									autocomplete="off"
									:class="{
										'bd-error': errors.length,
									}"
									@keydown="preventAngleBracket($event)"
								/>
								<small
									v-if="errors"
									class="text-error"
								>
									{{ errors[0] }}
								</small>
							</ValidationProvider>
							<ValidationProvider
								v-slot="{ errors }"
								:rules="{
									max: 31,
									required: true,
								}"
								class="col form-field"
								name="Last Name"
								tag="div"
							>
								<label
									for="last-name"
									class="required"
								>
									Last Name
								</label>
								<input
									id="last-name"
									v-model.trim="form.lastName"
									class="form-input"
									type="text"
									name="last-name"
									required
									autocomplete="off"
									:class="{
										'bd-error': errors.length,
									}"
									@keydown="preventAngleBracket($event)"
								/>
								<small
									v-if="errors"
									class="text-error"
								>
									{{ errors[0] }}
								</small>
							</ValidationProvider>
						</div>

						<!-- address -->
						<template v-if="hasAddressFields">
							<div class="row">
								<ValidationProvider
									v-slot="{ errors }"
									:rules="{
										max: 31,
										required: true,
									}"
									class="col form-field"
									name="Street Address"
									tag="div"
								>
									<label
										for="street-address"
										class="required"
									>
										Street Address
									</label>
									<input
										id="street-address"
										v-model.trim="form.address"
										class="form-input"
										type="text"
										name="street-address"
										required
										autocomplete="off"
										:class="{
											'bd-error': errors.length,
										}"
										@keydown="preventAngleBracket($event)"
									/>
									<small
										v-if="errors"
										class="text-error"
									>
										{{ errors[0] }}
									</small>
								</ValidationProvider>
							</div>
							<div class="row">
								<ValidationProvider
									v-slot="{ errors }"
									:rules="{
										max: 31,
										required: true,
									}"
									class="col-6 form-field"
									name="City"
									tag="div"
								>
									<label
										for="city"
										class="required"
									>
										City
									</label>
									<input
										id="city"
										v-model.trim="form.city"
										class="form-input"
										type="text"
										name="city"
										required
										autocomplete="off"
										:class="{
											'bd-error': errors.length,
										}"
										@keydown="preventAngleBracket($event)"
									/>
									<small
										v-if="errors"
										class="text-error"
									>
										{{ errors[0] }}
									</small>
								</ValidationProvider>

								<ValidationProvider
									v-slot="{ errors }"
									rules="required"
									class="col-2 form-field"
									name="State"
									tag="div"
								>
									<label
										for="state"
										class="required"
									>
										State
									</label>
									<select
										id="state"
										v-model="form.state"
										class="form-select"
										name="state"
										required
										autocomplete="off"
										:class="{
											'bd-error': errors.length,
										}"
									>
										<option :value="null">--</option>
										<option
											v-for="state in stateOptions"
											:key="state.value"
											:value="state.value"
										>
											{{ state.text }}
										</option>
									</select>
									<small
										v-if="errors"
										class="text-error"
									>
										{{ errors[0] }}
									</small>
								</ValidationProvider>
								<ValidationProvider
									v-slot="{ errors }"
									:rules="{
										required: true,
										zip: true,
										max: 10,
									}"
									class="col form-field"
									name="Zip Code"
									tag="div"
								>
									<label
										for="zipcode"
										class="required"
									>
										Zip Code
									</label>
									<TheMask
										id="zipcode"
										v-model.trim="form.zip"
										:mask="['#####', '#####-####']"
										class="form-input"
										type="text"
										name="zipcode"
										required
										autocomplete="off"
										:class="{
											'bd-error': errors.length,
										}"
										@keydown="preventAngleBracket($event)"
									/>
									<small
										v-if="errors"
										class="text-error"
									>
										{{ errors[0] }}
									</small>
								</ValidationProvider>
							</div>
						</template>

						<!-- email -->
						<div class="row">
							<ValidationProvider
								v-slot="{ errors }"
								:rules="{
									email: true,
									required:
										requireContactMethod ||
										(!form.phoneNumber &&
											formType === 'agentCustom'),
									max: 40,
								}"
								class="col form-field"
								name="Email"
								tag="div"
							>
								<label
									for="emailAddress"
									:class="{ required: !form.phoneNumber }"
								>
									Email
								</label>
								<input
									id="emailAddress"
									v-model.trim="form.emailAddress"
									class="form-input"
									type="email"
									name="emailAddress"
									autocomplete="off"
									:class="{
										'bd-error': errors.length,
									}"
									@keydown="preventAngleBracket($event)"
								/>
								<small
									v-if="errors"
									class="text-error"
								>
									{{ errors[0] }}
								</small>
							</ValidationProvider>
						</div>

						<!-- phone number -->
						<div
							v-if="hasPhoneFields"
							class="row"
						>
							<ValidationProvider
								v-slot="{ errors }"
								:rules="{
									min: 10,
									max: 10,
									required:
										requireContactMethod ||
										(!form.emailAddress &&
											formType === 'agentCustom'),
								}"
								name="Phone Number"
								class="col form-field"
								tag="div"
							>
								<label
									for="phoneNumber"
									:class="{ required: !form.emailAddress }"
								>
									Phone Number
								</label>
								<TheMask
									id="phoneNumber"
									v-model.trim="form.phoneNumber"
									:mask="'###-###-####'"
									class="form-input"
									type="tel"
									name="phoneNumber"
									autocomplete="off"
									:class="{
										'bd-error': errors.length,
									}"
									@keydown="preventAngleBracket($event)"
								/>
								<small
									v-if="errors"
									class="text-error"
								>
									{{ errors[0] }}
								</small>
							</ValidationProvider>
						</div>

						<!-- birth date -->
						<div
							v-if="hasBirthDateFields"
							class="row"
						>
							<ValidationProvider
								v-slot="{ errors }"
								:rules="{
									required: true,
									isAdult: true,
								}"
								class="col form-field"
								name="Date of Birth"
								tag="div"
							>
								<label
									for="birthDate"
									class="required"
								>
									Date of Birth
								</label>
								<div class="grouped">
									<TheMask
										id="birthDate"
										v-model="form.birthDate"
										placeholder="MM/DD/YYYY"
										:mask="'##/##/####'"
										:masked="true"
										class="form-input"
										type="text"
										name="birth-date"
										:class="{
											'bd-error': errors.length,
										}"
										@keydown="preventAngleBracket($event)"
									/>
									<div
										class="icon-only cursor-help"
										title="This helps us understand your immediate needs."
									>
										<CoreIconInfo />
									</div>
								</div>
								<small
									v-if="errors"
									class="text-error"
								>
									{{ errors[0] }}
								</small>
							</ValidationProvider>
						</div>

						<!-- language preference -->
						<div
							v-if="hasLanguageFields"
							class="row"
						>
							<div class="col form-field">
								<label for="language">
									Language Preference:
								</label>
								<select
									id="language"
									v-model="form.languagePref"
									class="select-small"
									autocomplete="off"
								>
									<option :value="null">--</option>
									<option
										v-for="language in languageOptions"
										:key="language"
										:value="language"
									>
										{{ language }}
									</option>
								</select>
							</div>
						</div>

						<!-- contact method preference -->
						<div
							v-if="hasBestMethodFields"
							class="row"
						>
							<div class="col">
								<label for="best-contact-method">
									Best Method to Contact:
								</label>
								<select
									id="best-contact-method"
									v-model="form.contactMethod"
									class="select-small"
									autocomplete="off"
								>
									<option :value="null">--</option>
									<option
										v-for="method in contactMethodOptions"
										:key="method.value"
										:value="method.value"
									>
										{{ method.text }}
									</option>
								</select>
							</div>
						</div>

						<!-- contact time preference -->
						<div
							v-if="hasBestTimeFields"
							class="row"
						>
							<div class="col form-field">
								<label for="best-time-to-call"
									>Best Time of Day to Call:</label
								>
								<select
									id="best-time-to-call"
									v-model="form.bestTimeToCall"
									class="select-small"
									autocomplete="off"
								>
									<option :value="'Not Provided'">--</option>
									<option
										v-for="time in contactTimeOptions"
										:key="time.value"
										:value="time.value"
									>
										{{ time.text }}
									</option>
								</select>
							</div>
						</div>

						<!-- linkedin -->
						<div
							v-if="hasLinkedinFields"
							class="row"
						>
							<ValidationProvider
								v-slot="{ errors }"
								:rules="{
									max: 50,
									linkedin: true,
								}"
								class="col form-field"
								name="LinkedIn"
								tag="div"
							>
								<label for="linkin-url">LinkedIn:</label>
								<input
									id="linkin-url"
									v-model.trim="form.linkedinUrl"
									class="form-input"
									name="linkedin-url"
									type="text"
									autocomplete="off"
									:class="{
										'bd-error': errors.length,
									}"
								/>
								<small
									v-if="errors"
									class="text-error"
								>
									{{ errors[0] }}
								</small>
							</ValidationProvider>
						</div>

						<!-- interests -->
						<div
							v-if="hasInterestFields"
							class="row row--interests"
						>
							<div class="col">
								<p>Interests:</p>
								<div
									v-for="{ value } in interestOptions"
									:key="value"
									class="interests-field"
								>
									<label
										:key="value"
										:for="value"
									>
										<input
											:id="value"
											v-model="form.leadConcerns"
											:value="value"
											type="checkbox"
											autocomplete="off"
										/>
										{{ value }}
									</label>
								</div>
							</div>
						</div>

						<!-- custom fields -->
						<template v-if="hasOpenTextFields">
							<ValidationProvider
								v-slot="{ errors }"
								rules="max:150"
								name="Custom Field"
								tag="div"
								class="row row--open-text"
							>
								<div class="col">
									<label for="openText">
										{{ openTextFieldLabel || "No Label" }}
									</label>
									<input
										id="openText"
										v-model.trim="form.openText"
										name="openText"
										type="text"
										:class="{
											'bd-error': errors.length,
										}"
										autocomplete="off"
										@keydown="preventAngleBracket($event)"
									/>
									<small
										v-if="errors"
										class="text-error"
									>
										{{ errors[0] }}
									</small>
								</div>
							</ValidationProvider>
						</template>

						<!-- controls -->
						<div class="row">
							<div class="col">
								<button
									type="submit"
									value="submit"
									class="button"
								>
									{{
										formType === "agentNewsletter"
											? "Sign Up For My Newsletter"
											: "Submit"
									}}
								</button>
							</div>
						</div>
						<div class="form__debug">
							<p v-if="marketerNumber">
								MarketerID: {{ marketerNumber }}
							</p>
							<p v-if="orgUnitCD">GO Code: {{ orgUnitCD }}</p>
						</div>
					</form>
				</ValidationObserver>
			</div>
		</div>
	</div>
</template>

<script>
import axios from "axios";
// Swagger schema for the API we are submitting to
// https://app.swaggerhub.com/apis/azimnitski/Lead/1.0.4

export default {
	name: "CoreBlockForm",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		dataSite: {
			type: Object,
			default: () => {},
		},
	},
	data() {
		return {
			form: {
				// fields that are always included
				firstName: null,
				lastName: null,
				address: null,
				city: null,
				state: null,
				zip: null,
				emailAddress: null,
				phoneNumber: null,
				birthDate: null,

				// fields that are toggled on
				openText: null,
				languagePref: null,
				bestTimeToCall: null,
				contactMethod: null,
				linkedinUrl: null,
				leadConcerns: [],

				// the url of this page -- this isn't ref'd anywhere on NYL endpoint. Can add later if needed.
				// pageUrl: null,

				// tells us where the lead came from i.e. google, bing, direct
				leadSource: "FUSION", // always fusion

				sourceCode: null, // values here: https://fusion92.sharepoint.com/:x:/s/NewYorkLife/EXkXq09zb4BFl-3OFknfg88BOrs99Jvk3YupzWfT5KWX2w

				// no idea what these fields are for

				leadProcessType: null, // agentPersWeb, goWeb, goWebRec
				campaignProgramCode: null, // C4, A2
				campaignCode: null, // PAW667, PGW668, UL0235

				// these fields are not included in the backend test
				// which was successfully returning a 200
			},
			status: null, // null, "success", "fail"
			stateOptions: [
				{ value: "AL", text: "Alabama" },
				{ value: "AK", text: "Alaska" },
				{ value: "AZ", text: "Arizona" },
				{ value: "AR", text: "Arkansas" },
				{ value: "CA", text: "California" },
				{ value: "CO", text: "Colorado" },
				{ value: "CT", text: "Connecticut" },
				{ value: "DC", text: "District of Columbia" },
				{ value: "DE", text: "Delaware" },
				{ value: "FL", text: "Florida" },
				{ value: "GA", text: "Georgia" },
				{ value: "HI", text: "Hawaii" },
				{ value: "ID", text: "Idaho" },
				{ value: "IL", text: "Illinois" },
				{ value: "IN", text: "Indiana" },
				{ value: "IA", text: "Iowa" },
				{ value: "KS", text: "Kansas" },
				{ value: "KY", text: "Kentucky" },
				{ value: "LA", text: "Louisiana" },
				{ value: "ME", text: "Maine" },
				{ value: "MD", text: "Maryland" },
				{ value: "MA", text: "Massachusetts" },
				{ value: "MI", text: "Michigan" },
				{ value: "MN", text: "Minnesota" },
				{ value: "MS", text: "Mississippi" },
				{ value: "MO", text: "Missouri" },
				{ value: "MT", text: "Montana" },
				{ value: "NE", text: "Nebraska" },
				{ value: "NV", text: "Nevada" },
				{ value: "NH", text: "New Hampshire" },
				{ value: "NJ", text: "New Jersey" },
				{ value: "NM", text: "New Mexico" },
				{ value: "NY", text: "New York" },
				{ value: "NC", text: "North Carolina" },
				{ value: "ND", text: "North Dakota" },
				{ value: "OH", text: "Ohio" },
				{ value: "OK", text: "Oklahoma" },
				{ value: "OR", text: "Oregon" },
				{ value: "PA", text: "Pennsylvania" },
				{ value: "RI", text: "Rhode Island" },
				{ value: "SC", text: "South Carolina" },
				{ value: "SD", text: "South Dakota" },
				{ value: "TN", text: "Tennessee" },
				{ value: "TX", text: "Texas" },
				{ value: "UT", text: "Utah" },
				{ value: "VT", text: "Vermont" },
				{ value: "VA", text: "Virginia" },
				{ value: "WA", text: "Washington" },
				{ value: "WV", text: "West Virginia" },
				{ value: "WI", text: "Wisconsin" },
				{ value: "WY", text: "Wyoming" },
			],
			contactTimeOptions: [
				{ value: "Morning", text: "Morning" },
				{ value: "Afternoon", text: "Afternoon" },
				{ value: "Evening", text: "Evening" },
			],
			contactMethodOptions: [
				{ value: "Email", text: "Email" },
				{ value: "Phone", text: "Phone" },
			],
			languageOptions: [
				"English",
				"Cantonese",
				"Farsi",
				"French",
				"Fukienese",
				"German",
				"Gujarati",
				"Hebrew",
				"Hindi",
				"Japanese",
				"Korean",
				"Lao",
				"Mandarin",
				"Polish",
				"Portuguese",
				"Punjabi",
				"Russian",
				"Shanghainese",
				"Spanish",
				"Tagalog",
				"Taiwanese",
				"Urdu",
				"Vietnamese",
			],
			userAge: null,
			baseURL:
				process.env.NUXT_ENV_API_BASE_URL ||
				"https://localhost:7283/api",
		};
	},
	computed: {
		isSubmitted() {
			return this.status !== null;
		},

		formType() {
			return this.settings.formType;
		},

		hasInterestFields() {
			if (this.settings.formType !== "agentNewsletter") {
				return this.settings?.interestsFieldset;
			}
			return false;
		},
		interestOptions() {
			if (this.hasInterestFields) {
				if (
					this.settings.formType === "goConsumer" ||
					this.settings.formType === "agentConsumer" ||
					this.settings.formType === "agentCustom"
				) {
					return [
						{ value: "Life Insurance" },
						{
							value: "Retirement Planning",
						},
						{
							value: "Retirement and Other Savings",
						},
						{
							value: "Estate, Trust & Legacy Planning",
						},
					];
				}
				if (this.settings.formType === "goRecruiter") {
					return [
						{
							value: "Financial Professional",
						},
						{ value: "College Intern" },
						{ value: "Fast Track" },
						{ value: "Management" },
					];
				}
			}
			return [];
		},

		hasAddressFields() {
			if (
				this.formType === "agentNewsletter" ||
				this.formType === "agentCustom"
			) {
				return false;
			}
			return true;
		},

		hasPhoneFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return true;
		},

		hasLanguageFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return !!this.settings?.languageFieldset;
		},

		hasBestTimeFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return !!this.settings?.bestTimeFieldset;
		},

		hasBestMethodFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return !!this.settings?.bestMethodFieldset;
		},
		hasLinkedinFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return !!this.settings?.linkedinFieldset;
		},
		hasBirthDateFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return (
				this.formType === "goConsumer" ||
				this.formType === "agentConsumer"
			);
		},
		hasOpenTextFields() {
			if (this.formType === "agentNewsletter") {
				return false;
			}
			return !!this.settings?.openTextFieldset;
		},
		openTextFieldLabel() {
			return this.settings?.openTextLabel || "";
		},
		marketerNumber() {
			if (
				this.formType === "goConsumer" ||
				this.formType === "goRecruiter"
			) {
				return null;
			}
			return this.dataSite.user?.ddcUserData?.marketerNo || null;
		},
		orgUnitCD() {
			if (
				this.formType === "agentCustom" ||
				this.formType === "agentConsumer" ||
				this.formType === "agentNewsletter"
			) {
				return null;
			}
			return this.dataSite.user?.ddcUserData?.orgUnitCode || null;
		},
		hiddenCodes() {
			switch (this.settings?.formType) {
				case "agentConsumer":
				case "agentCustom":
				case "agentNewsletter":
					return {
						leadProcessType: "agentPersWeb",
						campaignProgramCode: "C4",
						campaignCode: "PAW667",
						sourceCode: "4M5B6R",
					};
				case "goConsumer":
					return {
						leadProcessType: "goWeb",
						campaignProgramCode: "C4",
						campaignCode: "PGW668",
						sourceCode: "4M5B6S",
					};
				case "goRecruiter":
					return {
						leadProcessType: "goWebRec",
						campaignProgramCode: "A2",
						campaignCode: "UL0235",
						sourceCode: "4M5B6Q",
					};
			}
			return {
				leadProcessType: null,
				campaignProgramCode: null,
				campaignCode: null,
				sourceCode: null,
			};
		},
		requireContactMethod() {
			if (
				this.formType === "agentConsumer" ||
				this.formType === "goRecruiter" ||
				this.formType === "goConsumer"
			) {
				return true;
			}
			return false;
		},
		onSuccessMessage() {
			if (this.formType === "goRecruiter") {
				return "Thank you for your interest in joining our team at New York Life. A member of our team will contact you soon.";
			}
			if (this.formType === "agentNewsletter") {
				return "Thank you for reaching out and signing up for our newsletter.";
			}
			return "Thank you for reaching out. A member of our team will contact you soon.";
		},
	},
	watch: {
		form(val) {
			this.form = val.replace(/\W/g, "");
		},
	},
	async mounted() {
		await this.initializeReCAPTCHA();
	},
	beforeDestroy() {
		this.$recaptcha.destroy();
	},
	methods: {
		/*
			once the backend returns better responses
			this should be updated to display something
			more useful for the user
		*/
		async submit() {
			if (await this.passesReCAPTCHA()) {
				this.status = "pending";

				try {
					const payload = {
						...this.form,
						...this.hiddenCodes,
						marketerNumber: this.marketerNumber,
						orgUnitCD: this.orgUnitCD,
						siteId: this.dataSite.id,
						acf2id: this.acf2id,
					};

					if (payload.openText) {
						payload.customFields = [
							{
								id: "1",
								label: this.openTextFieldLabel,
								value: payload.openText,
							},
						];
					} else if (this.formType === "agentNewsletter") {
						payload.customFields = [
							{
								id: "1",
								label: "call to action",
								value: "newsletter",
							},
						];
					}
					// nyl requires semicolon delimited string
					if (payload.leadConcerns) {
						const interestString = payload.leadConcerns.join(";");
						payload.leadConcerns = interestString;
					}

					delete payload.openText;
					const response = await axios.post(
						`${this.baseURL}/leads/submit`,
						payload
					);
					if (response.status === 200) {
						this.status = "success";
					} else {
						this.status = "fail";
					}
				} catch {
					this.status = "fail";
				}
			}
			this.$recaptcha.reset();
		},
		async passesReCAPTCHA() {
			try {
				const responseToken = await this.$recaptcha.execute("form");
				// pass token to backend endpoint returning pass/fail
				const response = await axios.post(
					`${this.baseURL}/tooling/submit/recaptcha`,
					{ responseToken }
				);
				return response.data?.success || false;
			} catch (e) {
				console.log(e);
			}
			return false;
		},
		async initializeReCAPTCHA() {
			try {
				await this.$recaptcha.init();
			} catch (e) {
				console.error(e);
			}
		},
		preventAngleBracket(event) {
			if (/<|>/gi.test(event.key)) {
				event.preventDefault();
			}
		},
	},
};
</script>

<style lang="scss">
.form__debug {
	display: none;
}
.block_form {
	text-align: var(--_core__body_text-align);
	.cursor-help {
		cursor: help;
	}
}
</style>
