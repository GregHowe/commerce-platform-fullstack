<template>
	<v-container fluid>
		<v-row
			><v-col cols="12">
				<h1 class="my-8">Migration Template</h1>
				<v-stepper v-model="currentStep"
					><v-stepper-header>
						<v-stepper-step
							v-for="(step, i) in steps"
							:key="step.component"
							:step="i + 1"
							@click="setStep(i + 1)"
						>
							{{ step.label }}
						</v-stepper-step>
					</v-stepper-header>
					<v-stepper-items>
						<v-stepper-content
							v-for="(step, i) in steps"
							:key="step.component"
							:step="i + 1"
						>
							<OnboardingNylStep :step="step" />
						</v-stepper-content>
					</v-stepper-items>
				</v-stepper> </v-col
		></v-row>
	</v-container>
</template>

<script>
import { mapState, mapMutations } from "vuex";

export default {
	data: () => ({
		steps: [
			{
				overline: "SEARCH",
				label: "Load an Agent",
				subhead:
					"Enter an email or search by MarketerID to load the agent data.",
				component: "AgentSearch",
			},
			{
				overline: "AGENT",
				label: "Agent Details",
				subhead: "Configure details of the agent, the user.",
				component: "AgentDetails",
			},
			{
				overline: "BUILD",
				label: "Site Settings",
				subhead: "Enter in the site-specific details.",
				component: "SiteSettings",
			},
			{
				overline: "SITE",
				label: "Site Customization",
				subhead: "Determine the site-specific customizations.",
				component: "SiteCustomization",
			},
		],
		valid: true,
		name: "",
		nameRules: [
			(v) => !!v || "Name is required",
			(v) =>
				(v && v.length <= 10) || "Name must be less than 10 characters",
		],
		email: "",
		emailRules: [
			(v) => !!v || "E-mail is required",
			(v) => /.+@.+\..+/.test(v) || "E-mail must be valid",
		],
		select: null,

		calculatorItems: [
			"test1",
			"test2",
			"test3",
			"test4",
			"test5",
			"test6",
			"test7",
			"test8",
			"test9",
			"test10",
			"test11",
		],
		pageItems: [
			"test",
			"test",
			"test",
			"test",
			"test",
			"test",
			"test",
			"test",
			"test",
			"test",
			"test",
		],
		checkbox: false,
	}),
	computed: {
		...mapState({
			currentStep: (state) => state.migration.currentStep,
		}),
	},
	methods: {
		...mapMutations({
			setStep: "migration/setStep",
		}),
		validate() {
			this.$refs.form.validate();
		},
		reset() {
			this.$refs.form.reset();
		},
		resetValidation() {
			this.$refs.form.resetValidation();
		},
	},
};
</script>
