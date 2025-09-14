<template>
	<v-container>
		<v-row>
			<v-col
				cols="12"
				md="2"
				class="grey lighten-5"
			>
				<v-card
					outlined
					color="transparent"
				>
					<v-list-item three-line>
						<v-list-item-content>
							<div class="text-overline mb-4">
								{{ step.overline }}
							</div>
							<v-list-item-title class="text-h5 mb-1">
								{{ step.label }}
							</v-list-item-title>
							<v-list-item-subtitle>{{
								step.subhead
							}}</v-list-item-subtitle>
						</v-list-item-content>
					</v-list-item>
				</v-card>
			</v-col>
			<v-col
				cols="12"
				md="10"
			>
				<component :is="`OnboardingNylStep${step.component}`" />
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
export default {
	props: {
		step: {
			type: Object,
			default: () => ({
				component: "AgentSearch",
				overline: "",
				label: "",
				subhead: "",
			}),
		},
	},
};
</script>
