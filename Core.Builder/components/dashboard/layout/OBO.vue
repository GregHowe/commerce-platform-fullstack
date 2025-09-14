<template>
	<v-container fluid>
		<v-row>
			<v-col
				cols="12"
				lg="5"
			>
				<h2>
					Welcome {{ firstName }}! You are working on behalf of
					{{ oboFullName }}.
				</h2>
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
export default {
	computed: {
		...mapGetters("user", {
			firstName: "firstName",
			oboUser: "oboUser",
		}),
		oboFullName() {
			return this.oboUser?.displayName || "Unnamed User";
		},
	},
	methods: {
		...mapActions("site", ["createSite", "removeSite"]),
	},
};
</script>
