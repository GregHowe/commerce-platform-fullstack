<template>
	<v-card
		min-height="100%"
		outlined
	>
		<v-card-text>
			<v-text-field
				v-model="email"
				label="Email"
				outlined
			></v-text-field>
			<h3
				align="center"
				class="mb-4"
			>
				OR
			</h3>
			<v-text-field
				v-model="marketerID"
				label="MarketerID"
				outlined
			></v-text-field>
			<v-btn
				color="primary"
				x-large
				@click="submit()"
			>
				Search for Agent
			</v-btn>
			<v-alert
				v-show="error"
				outlined
				class="my-4 animate__animated animate__fadeInUp"
				dense
				type="error"
				>{{ error }}</v-alert
			>
		</v-card-text>
	</v-card>
</template>
<script>
import { mapState, mapActions, mapMutations } from "vuex";
export default {
	data() {
		return {
			email: "",
			marketerID: "",
		};
	},
	computed: {
		...mapState({
			error: (state) => state.migration.error,
			currentStep: (state) => state.migration.currentStep,
		}),
	},
	methods: {
		...mapActions({
			SEARCH_AGENT: "migration/SEARCH_AGENT",
		}),
		...mapMutations({
			setStep: "migration/setStep",
		}),
		async submit() {
			await this.SEARCH_AGENT(this.email);
			if (!this.error) {
				this.setStep(this.currentStep + 1);
			}
		},
	},
};
</script>
