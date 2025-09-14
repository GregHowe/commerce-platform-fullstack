<template>
	<span v-if="isAvailable">
		<slot />
	</span>
</template>
<script>
/*
	makes it easy for us to restrict
	[x] things
	based on the user's role
*/
import { mapGetters } from "vuex";
export default {
	props: {
		restrictedTo: {
			type: Array,
			default: () => [],
		},
	},
	computed: {
		...mapGetters({
			userRole: "user/role",
		}),
		isAvailable() {
			// admin club always gets in
			try {
				if (
					this?.userRole &&
					this.userRole.toString().toLowerCase() == "admin"
				) {
					return true;
				}
				// available if no restrictedTo or restrictedTo includes user's role
				return (
					!this.restrictedTo.length ||
					this.restrictedTo.includes(this.userRole)
				);
			} catch (err) {
				console.error("Auth wrapper error: ", err);
			}
			return false;
		},
	},
};
</script>
