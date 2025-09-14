<template>
	<div class="container mt-12">
		<v-img
			src="/img/logo-black.svg"
			max-width="270px"
			alt="core 2.0"
		/>
		<h1>Successfully signed in!</h1>
		{{ auth }}
		{{ user }}
	</div>
</template>

<script>
import { mapState } from "vuex";
export default {
	layout: "blank",
	computed: {
		...mapState({
			auth: (state) => state.auth,
			user: (state) => state.user,
		}),
	},
};
</script>

<style
	lang="scss"
	scoped
>
.container {
	max-width: 1000px !important;
}
h1 {
	font-weight: 200;
	font-size: 48px;
}
</style>
