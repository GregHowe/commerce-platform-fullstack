<template>
	<nav
		v-if="crumbs.length > 1"
		aria-label="breadcrumb"
		:class="crumbClasses"
	>
		<ul class="breadcrumb__list">
			<li
				v-for="(crumb, index) in crumbs"
				:key="index"
				:class="listClasses"
			>
				<span
					v-if="index !== 0"
					class="breadcrumb__list-item--separator"
				>
					>
				</span>
				<NuxtLink
					:to="crumb.path"
					:aria-current="$route.name === crumb.name"
				>
					<span class="breadcrumb__list-item--title">{{
						crumb.title
					}}</span>
				</NuxtLink>
			</li>
		</ul>
	</nav>
</template>

<script>
export default {
	name: "CoreBlockBreadcrumbs",
	props: {
		settings: {
			type: Object,
			default: () => {},
		},
	},
	computed: {
		// this is pretty tricky, since routes in the builder are different than on the site
		// i think we need to find the current active page
		// and build the crumbs backwards..
		// like current page is page C, its parent is page B, and that parent is page A
		// woule end up pageA > pageB > pageC
		crumbs() {
			const fullPath = this.$route.fullPath;
			const params = fullPath.startsWith("/")
				? fullPath.substring(1).split("/")
				: fullPath.split("/");
			const crumbs = [];
			let path = "";
			params.forEach((param) => {
				path = `${path}/${param}`;
				const match = this.$router.match(path);
				if (match.name !== null) {
					crumbs.push({
						title: param.replace(/-/g, " "),
						...match,
					});
				}
			});
			return crumbs;
		},
		variants() {
			return this.settings?.variants || {};
		},
		isCompact() {
			return this.variants.breadcrumbVariant === "compact";
		},
		crumbClasses() {
			let classes = "breadcrumb";
			if (this.isCompact) {
				classes = `${classes} breadcrumb__compact`;
			}
			return classes;
		},
		listClasses() {
			let classes = "breadcrumb__list-item";
			if (this.isCompact) {
				classes = `${classes} breadcrumb__list-item--compact`;
			}
			return classes;
		},
	},
};
</script>

<style lang="scss">
.breadcrumb {
	width: 100%;
	padding-bottom: 5px;
	background-color: var(--_core__breadcrumb_background);
	padding-left: 12px;
	padding-right: 12px;
	&__compact {
		width: 37%;
		margin: 3rem 3rem 0 3rem;
	}
	&__list {
		display: flex;
		max-width: var(--_core__container_max-width);
		width: 100%;
		margin-right: auto;
		margin-left: auto;
		padding-left: 0 !important;
		&-item {
			text-transform: capitalize;
			list-style: none;
			margin-right: 0.6rem;
			&--compact {
				width: 4rem;
				white-space: nowrap;
				overflow: hidden;
				text-overflow: ellipsis;
				&:last-child {
					width: 15rem;
				}
			}
			&:hover,
			&:hover a {
				text-decoration: none;
				font-weight: 700;
				color: var(--_core__breadcrumb_hover-color);
				a {
					text-decoration: underline;
				}
			}
			& .nuxt-link-exact-active,
			.nuxt-link-active {
				font-weight: 700;
				text-decoration: none;
				color: var(--_core__breadcrumb_color);
				.breadcrumb__list-item--title {
					font-weight: 700;
				}
			}
			&--separator {
				margin: 0 10px 0 5px;
				font-family: var(--_core__breadcrumb_font-family);
				font-size: var(--_core__breadcrumb_font-size);
				color: var(--_core__breadcrumb_color);
				font-weight: 500;
				text-decoration: none;
			}
			&--title {
				font-family: var(--_core__breadcrumb_font-family);
				font-size: var(--_core__breadcrumb_font-size);
				color: var(--_core__breadcrumb_color);
			}
		}
	}
}
// breakpoint needs to correspond with Core.Builder/assets/scss/mixins.scss
// these mixins (breakpoint_up and breakpoint_down) are so we don't have to do this
// breakpoint_down seems to not work here though for some reason
// should fix it because it applies fake breakpoint classes for the builder to mock responsiveness
@media screen and (max-width: 768px) {
	.breadcrumb__compact {
		width: 100%;
		margin: 0;
	}
	.breadcrumb {
		padding-left: 26px;
		padding-right: 26px;
	}
	.breadcrumb__list-item {
		width: auto;
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
		&:last-child {
			width: 15rem;
		}
	}
}
</style>
