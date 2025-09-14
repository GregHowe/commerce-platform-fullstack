<template>
	<div style="max-width: 500px">
		<h3 class="font-weight-light">{{ user.displayName }}</h3>
		<h4>Permissions</h4>
		<v-chip
			v-for="permission in permissions"
			:key="permission"
			x-small
			class="ma-1"
			color="success"
			>#{{ permission }}</v-chip
		>
		<h4 class="mt-3">User information</h4>
		<v-data-table
			:headers="headers"
			:items="userFields"
			:items-per-page="25"
			class="elevation-1 mt-3"
		></v-data-table>
	</div>
</template>
<script>
import { mapState, mapGetters } from "vuex";
export default {
	data() {
		return {
			headers: [
				{ text: "Property", value: "property" },
				{ text: "Value", value: "value" },
			],
		};
	},
	computed: {
		...mapState({
			user: (state) => state?.auth?.user,
		}),
		...mapGetters({
			fullName: "user/fullName",
			permissions: "user/permissions",
		}),
		userFields() {
			try {
				if (this.user) {
					let fields = {
						...this.user,
					};
					return Object.keys(fields)
						.map((f) => {
							if (typeof fields[f] != "object") {
								return { property: f, value: fields[f] };
							}
							return {
								property: f,
								value: JSON.stringify(fields[f]),
							};
						})
						.filter((k) => k.value);
				}
				return [];
			} catch (err) {
				return [];
			}
		},
	},
};
</script>
