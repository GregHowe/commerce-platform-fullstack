<template>
	<div>
		<CoreBlock :settings="{ ...settings, type: mediaType }" />
	</div>
</template>

<script>
export default {
	name: "CoreBlockMedia",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
	computed: {
		mediaUrl() {
			return this.settings.src;
		},
		blockComponent() {
			switch (this.mediaType) {
				case "image":
					return "CoreBlockImage";
				case "file":
					return "CoreBlockFile";
				case "video":
					return "CoreBlockVideo";
			}
			return "CoreBlockUnknown";
		},
		isImage() {
			if (this.mediaUrl) {
				return (
					this.mediaUrl.endsWith(".png") ||
					this.mediaUrl.endsWith(".jpg") ||
					this.mediaUrl.endsWith(".jpeg") ||
					this.mediaUrl.endsWith(".webp") ||
					this.mediaUrl.endsWith(".gif") ||
					this.mediaUrl.endsWith(".svg")
				);
			}
			return false;
		},
		isFile() {
			if (this.mediaUrl) {
				return (
					this.mediaUrl.endsWith(".csv") ||
					this.mediaUrl.endsWith(".pdf") ||
					this.mediaUrl.endsWith(".html") ||
					this.mediaUrl.endsWith(".txt")
				);
			}
			return false;
		},
		isVideo() {
			if (this.mediaUrl) {
				return (
					this.mediaUrl.match("youtube.com/embed") ||
					this.mediaUrl.match("player.vimeo") ||
					this.mediaUrl.match("players.brightcove")
				);
			}
			return false;
		},
		mediaType() {
			if (this.isImage) {
				return "image";
			}
			if (this.isFile) {
				return "file";
			}
			if (this.isVideo) {
				return "video";
			}
			return null;
		},
	},
};
</script>
