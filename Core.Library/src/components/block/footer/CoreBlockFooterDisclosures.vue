<template>
	<CoreBlock
		v-if="showDisclosures"
		class="disclosures"
		:class="{ 'disclosures--mobile': isMobile }"
		:settings="accordionSettings"
	/>
</template>

<script>
import { renderData } from "@libraryHelpers/dataComponents.js";

export default {
	name: "CoreBlockFooterDisclosures",
	props: {
		site: {
			type: Object,
			required: true,
		},
		isMobile: {
			type: Boolean,
			default: false,
		},
	},
	data() {
		return {
			clonedSite: null,
			stateValues: [
				{ value: "AL", text: "Alabama" },
				{ value: "AK", text: "Alaska" },
				{ value: "AZ", text: "Arizona" },
				{ value: "AR", text: "Arkansas" },
				{ value: "CA", text: "California" },
				{ value: "CO", text: "Colorado" },
				{ value: "CT", text: "Connecticut" },
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
		};
	},
	computed: {
		user() {
			return this.site.user || {};
		},
		disclosures() {
			const disclosures = this.site.footer?.disclosures || [];
			return disclosures.map((item) => renderData(item, this.clonedSite));
		},
		showDisclosures() {
			return !!this.site.footer?.showDisclosures;
		},
		accordionSettings() {
			return {
				type: "accordion",
				variants: {},
				accordionText: "Disclosure",
				hasAccordion: true,
				blocks: [
					...this.disclosures.map((disclosure) => {
						return {
							type: "rich-text",
							variants: {
								size: "disclosure",
							},
							richText: disclosure,
						};
					}),
				],
				locked: null,
			};
		},
		sellingInsuranceLicenses() {
			return this.licenses({
				licenseLobCode: "80",
				busLicenseTpCode: "I",
				busEntityCode: "001",
			});
		},
		registeredAgentLicenses() {
			return this.licenses({ licenseLobCode: "30" });
		},
		eagleAdvisorLicenses() {
			return this.licenses({ eagleData: "Y" });
		},
		statesSellingInsurance() {
			return this.states(this.sellingInsuranceLicenses);
		},
		sellingInsuranceStateText() {
			return this.stateText(this.sellingInsuranceLicenses);
		},
		statesRegisteredAgent() {
			return this.states(this.registeredAgentLicenses);
		},
		registeredAgentStateText() {
			return this.stateText(this.registeredAgentLicenses);
		},
		statesEagleFinancial() {
			return this.states(this.eagleAdvisorLicenses);
		},
		eagleFinancialStateText() {
			return this.stateText(this.eagleAdvisorLicenses);
		},
	},
	created() {
		this.addDataToSite();
	},
	methods: {
		stateText(states) {
			return states.length > 1 ? "states" : "state";
		},
		licenses(args = {}) {
			const licenses = this.user.ddcUserData?.ddcLicenseData || [];
			const filtered = licenses.filter((license) => {
				const expireDate = new Date(license.licenseExpiryDt).getTime();
				const issueDate = new Date(license.licenseIssueDt).getTime();
				const today = new Date().getTime();
				const query = Object.keys(args).every((key) => {
					return license[key] === args[key];
				});
				return query && expireDate >= today && issueDate <= today;
			});
			// only one per state (remove duplicate valid states)
			return filtered.filter(
				(obj, index, self) =>
					index ===
					self.findIndex(
						(t) => t.stateCountyCode === obj.stateCountyCode
					)
			);
		},
		states(licenses) {
			// create a string of states based on the license data
			const appendableStates = ["CA", "AR"];
			const stateArray = licenses
				.map((license) => {
					const state = license.stateCountyCode;
					// append the license number to certain states
					if (appendableStates.includes(state)) {
						return `${state} (${state} Insurance License #${license.licenseIdNumber})`;
					}
					// if the state is a number, use "FL" instead
					if (!isNaN(state)) {
						return "FL";
					}
					return state;
				})
				.join(",")
				.split(",");
			// Sort the array alphabetically
			stateArray.sort();
			// if they are licensed in more than one state, join them with "and" before the last item
			if (stateArray.length > 1) {
				const lastItem = stateArray.pop();
				return `${stateArray.join(", ")}, and ${lastItem}`;
			}

			// if they are licensed in only one state, return the full name of the state
			const singleState = stateArray[0].split(" ")[0].trim();
			const fullState = this.stateValues.find((state) => {
				return state.value === singleState;
			});
			if (fullState) {
				const license = licenses.find(
					(l) => l.stateCountyCode === fullState.value
				);
				if (
					license?.licenseIdNumber &&
					appendableStates.includes(license.stateCountyCode.trim())
				) {
					return `${fullState.text} (${fullState.text} Insurance License #${license.licenseIdNumber})`;
				}
				return fullState.text;
			}
			return "";
		},
		addDataToSite() {
			// here we can add various data points to use in the disclosure via mustache templates
			// needs to be done in created so that the computed data is available in the mustache templates
			this.clonedSite = {
				statesSellingInsurance: this.statesSellingInsurance,
				statesRegisteredAgent: this.statesRegisteredAgent,
				sellingInsuranceStateText: this.sellingInsuranceStateText,
				registeredAgentStateText: this.registeredAgentStateText,
				eagleFinancialStateText: this.eagleFinancialStateText,
				statesEagleFinancial: this.statesEagleFinancial,
				...this.site,
			};
		},
	},
};
</script>

<style lang="scss">
.disclosures {
	color: var(--core__footer-font_color);
	padding: 1.5rem 0;
	.block__body {
		font-size: var(--_core__disclosure_font-size);
	}
	&--mobile {
		padding: 0 30px;
	}
}
</style>
