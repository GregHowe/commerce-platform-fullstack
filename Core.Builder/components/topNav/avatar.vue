<template>
	<div v-show="fullName">
		<ForgeTooltip left>
			<template #trigger>
				<v-btn
					id="logoutbtn"
					text
					class="text-caption"
					@click="logout"
					><v-icon>mdi-account</v-icon>Logout
				</v-btn>
			</template>
			<template #default> {{ fullName }}, {{ userRole }} </template>
		</ForgeTooltip>
	</div>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	computed: {
		...mapGetters({
			userRole: "user/role",
			fullName: "user/fullName",
		}),
	},
	methods: {
		logout() {
			this.$router.push({ path: "/auth/logout" });
		},
	},
};
</script>
<style scoped>
.logoutbtn {
	cursor: pointer;
}
.logoutbtn:hover {
	text-decoration: underline;
}
</style>
