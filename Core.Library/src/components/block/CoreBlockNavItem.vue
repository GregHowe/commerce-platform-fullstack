<template>
	<li
		class="nav__item--container"
		@mouseover="hoverPageId = navitem.linkUrl"
		@mouseout="hoverPageId = null"
		@click="$emit('click')"
	>
		<!-- An Internal Link-->
		<NuxtLink
			v-if="navitem.isInternalLink && !navitem.isFolder"
			:to="fullLinkUrl"
			:target="target"
			role="menuitem"
			class="navbar_link"
		>
			{{ navitem.linkText }}
		</NuxtLink>
		<!-- An External Link-->
		<a
			v-else-if="!navitem.isFolder"
			role="menuitem"
			class="navbar_link non-nuxt"
			:target="target"
			:href="fullLinkUrl"
			>{{ navitem.linkText }}</a
		>
		<!-- A Folder, contains subpages, but no root level link-->
		<div
			v-else
			role="menuitem"
			class="navbar_link"
		>
			{{ navitem.linkText }}
		</div>
		<ul
			v-show="hasChildren"
			class="navbar_nav navbar_nav--subnav block_main-nav--subnav_open"
		>
			<CoreBlockNavItem
				v-for="page2 in subPages"
				:key="page2.linkText + page2.linkUrl"
				:navitem="page2"
				:parent-link-url="fullLinkUrl"
				:site="site"
				class="navbar_item"
			/>
		</ul>
	</li>
</template>

<script>
export default {
	name: "CoreBlockNavItem",
	props: {
		site: {
			type: Object,
			default: () => {},
		},
		parentLinkUrl: {
			type: String,
			default: "",
		},
		navitem: {
			type: Object,
			default: () => {
				return {
					id: "",
					linkText: "",
					isInternalLink: "",
					openInNewTab: false,
					linkUrl: "",
					isFolder: "",
				};
			},
		},
	},
	data() {
		return {
			showDropDown: false,
		};
	},
	computed: {
		navItems() {
			return this?.site?.navigation?.links || [];
		},
		fullLinkUrl() {
			if (this.navitem.isInternalLink) {
				return `${
					this.parentLinkUrl !== "/" && this.parentLinkUrl !== ""
						? this.parentLinkUrl
						: ""
				}${this.navitem.linkUrl}`;
			}
			return this.navitem.linkUrl;
		},
		subPages() {
			const subPages = this?.navItems.filter(
				(nI) => nI.parent == this.navitem.id
			);
			return subPages;
		},
		target() {
			return this.navitem.openInNewTab ? "_blank" : "_self";
		},
		hasChildren() {
			return this.subPages?.length;
		},
		isMobile() {
			if (this.isBuilderMobile) {
				return true;
			}
			// breakpoint needs to correspond with Core.Builder/assets/scss/mixins.scss
			const breakpoint = 768;
			return this.windowWidth && this.windowWidth < breakpoint;
		},
	},
	methods: {
		toggleDropDown() {
			this.showDropDown = !this.showDropDown;
		},
	},
};
</script>

<style lang="scss">
.subnav {
	font-family: var(--core__font_primary);
	width: 20%;
	text-align: left;
	padding-bottom: 24px;
	padding-right: 4px;
	& ul {
		padding-left: 0;
	}
	& a {
		color: var(--_core__navbar_color);
		text-decoration: none;
		:hover {
			text-decoration: underline;
		}
	}
	& li {
		list-style: none;
		padding-top: 12px;
		font-size: 0.8rem;
		width: auto;
	}
	& h3 {
		font-size: 1rem;
		margin: 0;
	}
	& svg {
		margin-left: 20px;
		margin-bottom: -6px;
		width: 14px;
	}
	&__mobile {
		margin-right: 0;
		text-align: left;
		& ul {
			padding-left: 0;
		}
		& a {
			font-family: var(--core__font_primary);
			color: var(--_core__navbar_color);
			text-decoration: none;
			:hover {
				text-decoration: underline;
			}
		}
		& li {
			display: flex;
			justify-content: space-between;
			list-style: none;
			padding-top: 12px;
			font-size: 0.8rem;
			width: auto;
			border-bottom: 1px solid #bcbcbc;
			padding: 16px 30px 16px 48px;
		}
		& h3 {
			display: flex;
			justify-content: space-between;
			font-size: 1rem;
			border-bottom: 1px solid #bcbcbc;
			padding: 16px 30px 16px 24px;
		}
		& svg {
			margin-left: 20px;
			margin-bottom: -1px;
		}
	}
}

.nav__item--container {
	position: relative;
}

.nav-arrow {
	position: absolute;
	right: 10px;
}

.right__arrow {
	transform: rotate(-90deg);
}

.rotate__arrow {
	transform: rotate(0deg);
}

.upward__arrow {
	transform: rotate(180deg);
}
</style>
