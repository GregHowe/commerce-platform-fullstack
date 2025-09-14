<template>
	<v-navigation-drawer
		v-bind="$attrs"
		color="secondary lighten-2"
	>
		<template #prepend>
			<slot name="prepend"></slot>
		</template>
		<slot>
			<p>Contents of Drawer go here.</p>
		</slot>
		<template #append>
			<slot name="append"></slot>
		</template>
	</v-navigation-drawer>
</template>
<script>
export default {
	inheritAttrs: false,
};
</script>
