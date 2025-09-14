<template>
	<div v-if="showDisclosures">
		<v-list-item>
			<label>Footer Disclosures</label>
		</v-list-item>
		<v-list-item
			v-for="disclosure in disclosures"
			:key="disclosure.idx"
		>
			<EditorRichText
				:value="disclosure.item"
				@input="
					(newValue) => {
						setWorkingSiteSetting({
							key: `footer.disclosures[${disclosure.idx}]`,
							value: newValue,
						});
					}
				"
			/>
		</v-list-item>
		<GlobalAuthWrapper>
			<v-list-item>
				<ForgeButton
					text
					@click="addDisclosure"
				>
					<CoreIconPlus />
					Add Disclosure
				</ForgeButton>
			</v-list-item>
			<v-list-item>
				<ForgeButton
					color="error"
					@click="resetDisclosures"
				>
					<CoreIconReplay />
					Reset Disclosures
				</ForgeButton>
			</v-list-item>
			<v-list-item class="text-caption"
				>This resets this sites disclosures based on the site user's
				data</v-list-item
			>
		</GlobalAuthWrapper>
	</div>
</template>

<script>
import { mapActions, mapGetters, mapMutations, mapState } from "vuex";

export default {
	name: "EditorFooterDisclosures",
	data() {
		return {
			disclosures: [],
		};
	},
	computed: {
		...mapState("library", ["content"]),
		...mapState("site", ["storedSite"]),
		...mapGetters("site", ["getWorkingSiteSetting"]),
		showDisclosures() {
			return this.getWorkingSiteSetting("footer.showDisclosures");
		},
		user() {
			return this.storedSite?.user || {};
		},
		isDBA() {
			return !!this.user.ddcUserData?.dba1Nm || false;
		},
		isGO() {
			return this.user.employeeType === "GO";
		},
		isRegisteredRep() {
			return this.user.ddcUserData?.regRepInd === "Y" || false;
		},
		isEagleAdvisor() {
			return this.user.ddcUserData?.eagleInd === "Y" || false;
		},
		isNautilusMember() {
			return this.user.ddcUserData?.nautilusInd === "Y" || false;
		},
		isMDRTMember() {
			return (
				this.user.ddcUserData?.mdrtLastYear >=
					new Date().getFullYear() || false
			);
		},
		isLicensedToSellInsurance() {
			const licenses = this.user.ddcUserData?.ddcLicenseData || [];
			return licenses.some((license) => {
				const licenceDate = new Date(license.licenseExpiryDt);
				return licenceDate.getTime() > new Date().getTime();
			});
		},
		GODisclosure() {
			// if General Office, only show General Office disclosure
			const goDisclosure = this.content.find(
				(item) =>
					item.type === "disclosure" &&
					item.title.toLowerCase().includes("general office")
			);
			if (goDisclosure) {
				return `<p>${goDisclosure.presetSettings.text}</p>`;
			}
			return null;
		},
		agentDisclosures() {
			const agentDisclosures = this.content.filter(
				(item) =>
					item.type === "disclosure" &&
					!item.title.toLowerCase().includes("general office")
			);
			if (agentDisclosures) {
				return agentDisclosures.map((item) => {
					return {
						title: item.title,
						text: `<p>${item.presetSettings.text}</p>`,
					};
				});
			}
			return null;
		},
	},
	mounted() {
		this.setDisclosures();
	},
	methods: {
		...mapMutations("site", ["setWorkingSiteSetting"]),
		...mapActions("library", ["getContentByType"]),
		async resetDisclosures() {
			await this.setWorkingSiteSetting({
				key: "footer.disclosures",
				value: [],
			});
			await this.setDisclosures();
		},
		addDisclosure() {
			const nextIndex = this.disclosures?.length || 0;
			this.setWorkingSiteSetting({
				key: `footer.disclosures[${nextIndex}]`,
				value: "",
			});
			return this.setDisclosures();
		},
		async setDisclosures() {
			let disclosures = [
				...this.getWorkingSiteSetting("footer.disclosures"),
			];

			if (!disclosures.length) {
				// get all disclosures from library
				await this.getContentByType("disclosure");

				if (this.isGO) {
					// if General Office, only show General Office disclosure
					if (this.GODisclosure) {
						disclosures.push(this.GODisclosure);
					}
				} else {
					if (this.agentDisclosures) {
						// make sure the disclosures are populated in the correct order
						if (this.isLicensedToSellInsurance) {
							const disclosure = this.agentDisclosures.find(
								(item) =>
									item.title
										.toLowerCase()
										.includes("license to sell insurance")
							);
							if (disclosure) {
								disclosures.push(disclosure.text);
							}
						}
						if (this.isRegisteredRep) {
							const disclosure = this.agentDisclosures.find(
								(item) =>
									item.title
										.toLowerCase()
										.includes("registered representative")
							);
							if (disclosure) {
								disclosures.push(disclosure.text);
							}
						}
						if (this.isEagleAdvisor) {
							const disclosure = this.agentDisclosures.find(
								(item) =>
									item.title
										.toLowerCase()
										.includes("eagle financial advisor")
							);
							if (disclosure) {
								disclosures.push(disclosure.text);
							}
						}
						if (this.isNautilusMember) {
							const disclosure = this.agentDisclosures.find(
								(item) =>
									item.title
										.toLowerCase()
										.includes("nautilus member")
							);
							if (disclosure) {
								disclosures.push(disclosure.text);
							}
						}
						if (this.isMDRTMember) {
							const disclosure = this.agentDisclosures.find(
								(item) =>
									item.title
										.toLowerCase()
										.includes("mdrt member")
							);
							if (disclosure) {
								disclosures.push(disclosure.text);
							}
						}
						if (this.isDBA) {
							const disclosure = this.agentDisclosures.find(
								(item) =>
									item.title
										.toLowerCase()
										.includes("ownership")
							);
							if (disclosure) {
								disclosures.push(disclosure.text);
							}
						}
						const disclosure = this.agentDisclosures.find((item) =>
							item.title.toLowerCase().includes("advise")
						);
						if (disclosure) {
							disclosures.push(disclosure.text);
						}
					}
				}
			}

			this.setWorkingSiteSetting({
				key: `footer.disclosures`,
				value: disclosures,
			});

			this.disclosures = disclosures?.map((item, idx) => {
				return { idx, item };
			});
		},
	},
};
</script>
