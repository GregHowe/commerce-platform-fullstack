<template>
	<v-chip large></v-chip>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	data() {
		return {
			roles: [
				{
					name: "Admin",
					color: "black",
					icon: "mdi-briefcase",
				},
				{
					name: "Agent",
					color: "blue",
					icon: "mdi-face-agent",
				},
				{
					name: "Compliance",
					color: "orange",
					icon: "mdi-shield-star",
				},
				{
					name: "ContentLibrarian",
					color: "green",
					icon: "mdi-library",
				},
			],
		};
	},
	computed: {
		...mapGetters({
			role: "user/role",
		}),
	},
};
</script>
