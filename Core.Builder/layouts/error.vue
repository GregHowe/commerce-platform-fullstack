<template>
	<div>
		<transition
			name="quickfade"
			mode="in-out"
		>
			<div class="ma-10">
				<div v-if="error.statusCode === 404">
					<h1>404 Page not found</h1>
					<p>
						{{ $route.path }} is not a valid location in this
						application.
					</p>
				</div>

				<div v-else>
					<h1>An error occured.</h1>
					<p>
						An error of type {{ error.statusCode }} was encountered.
						Please try again, or report this issue.
					</p>
				</div>
				<p>
					Contact
					<NuxtLink to="/support">support</NuxtLink> for further
					assistance.
				</p>
			</div>
		</transition>
	</div>
</template>

<script>
export default {
	layout: "LayoutError",
	props: {
		error: {
			type: Object,
			default: null,
		},
	},
};
</script>
