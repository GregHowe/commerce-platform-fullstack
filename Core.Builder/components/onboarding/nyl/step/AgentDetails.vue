<template>
	<v-card
		min-height="100%"
		outlined
	>
		<v-card-text>
			<v-tabs
				v-model="tab"
				dark
				background-color="black"
				slider-color="blue"
				grow
			>
				<v-tab
					v-for="fieldset in sortedAgentFields"
					:key="fieldset.title"
				>
					{{ fieldset.title }}
				</v-tab>
			</v-tabs>
			<v-tabs-items v-model="tab">
				<v-tab-item
					v-for="fieldset in sortedAgentFields"
					:key="fieldset.title"
					class="mt-4"
				>
					<h3 class="mb-8">{{ fieldset.title }}</h3>
					<v-row>
						<v-col
							v-for="field in fieldset.fields"
							:key="field.key"
							cols="12"
							class="pt-0"
							:md="field.cols"
						>
							<v-text-field
								:value="currentAgent[field.key]"
								dense
								outlined
								disabled
								v-bind="field"
								@input="
									(event) =>
										setAgentProperty({
											key: field.k,
											value: event.target.value,
										})
								"
							></v-text-field
						></v-col>
					</v-row>
				</v-tab-item>
			</v-tabs-items>
		</v-card-text>
	</v-card>
</template>
<script>
import { mapState, mapGetters, mapMutations } from "vuex";
export default {
	data() {
		return {
			tab: "",
		};
	},
	computed: {
		stateList() {
			return [];
		},
		...mapState({
			currentAgent: (state) => state.migration.currentAgent,
		}),
		...mapGetters({
			sortedAgentFields: "migration/sortedAgentFields",
		}),
	},
	methods: {
		...mapMutations({
			setAgentProperty: "migration/setAgentProperty",
		}),
	},
};
</script>
