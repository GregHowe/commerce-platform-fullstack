<template>
	<div>
		<DashboardEligibilityNotAuthorized v-if="!isEligible" />
		<div v-else>
			<ModalFirstTimeUser />
			<v-parallax
				dark
				src="/img/rectanglebg.jpg"
				height="240"
				class="px-4"
			>
				<v-row
					align="center"
					justify="center"
				>
					<v-col
						class="text-left px-12"
						cols="12"
					>
						<h1 style="font-size: 42px">
							<v-icon
								style="top: -5px"
								color="white"
								size="42"
							>
								mdi-home-outline
							</v-icon>
							Your Dashboard
						</h1>
						<h4 class="subheading">Welcome, {{ fullName }}.</h4>
					</v-col>
				</v-row>
			</v-parallax>
			<component :is="`DashboardLayout${roleFormatted}`" />
		</div>
	</div>
</template>
<script>
import { mapGetters } from "vuex";

export default {
	layout({ store }) {
		const isEligible = store.getters["user/isEligible"];
		if (isEligible) {
			return "headeronly";
		}
		return "blank";
	},
	data() {
		return {
			validRoles: ["Admin", "Field User", "OBO", "HO", "GO"],
			dialog: true,
		};
	},
	computed: {
		...mapGetters({
			fullName: "user/fullName",
			role: "user/role",
			isEligible: "user/isEligible",
		}),
		roleFormatted() {
			return this.validRoles.includes(this.role)
				? this.role.replace(/\s+/g, "")
				: "FieldUser";
		},
	},
};
</script>
