import EditorMedia from "~/components/editor/EditorMedia";
import { render } from "@testing-library/vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const renderComponent = (propsData = {}) => {
	const root = document.createElement("div");
	return render(EditorMedia, {
		container: document.body.appendChild(root),
		vuetify: new Vuetify(),
		propsData,
	});
};

describe("EditorMedia", () => {
	it("should exist", () => {
		renderComponent({});
	});

	it("should show buttons based on accepted types, and first item in array should be the active button", () => {
		const { container, getByText, queryByText } = renderComponent({
			acceptTypes: ["image", "file"],
		});

		// we have file and image buttons, but not video
		getByText("image");
		getByText("file");
		expect(queryByText("video")).not.toBeInTheDocument();

		// the first button in the button group has a value of "image" and is active
		const firstButton =
			container.querySelector("div.v-item-group").firstChild;
		expect(firstButton.className).toContain("active");
		expect(firstButton.value).toBe("image");
	});

	it("should not show the button group if only one accepted type is passed, but should show the editor of the accepted type", () => {
		const { container, queryByText } = renderComponent({
			acceptTypes: ["file"],
		});

		expect(queryByText("video")).not.toBeInTheDocument();
		expect(queryByText("image")).not.toBeInTheDocument();

		// we have the file editor automatically loaded
		const editor = container.querySelector("div.editor-file");
		expect(editor).toBeTruthy();
	});

	it("if video url is passed in, should display the video editor and not the button group", () => {
		const { container, queryByText } = renderComponent({
			value: "https://players.brightcove.com/455454",
		});

		expect(queryByText("video")).not.toBeInTheDocument();
		expect(queryByText("image")).not.toBeInTheDocument();
		expect(queryByText("file")).not.toBeInTheDocument();

		// we have the video editor automatically loaded
		const editor = container.querySelector("div.editor-video");
		expect(editor).toBeTruthy();
	});

	it("if image url is passed in, should display the image editor and not the button group", () => {
		const { container, queryByText } = renderComponent({
			value: "https://somethingsomething.png",
		});

		expect(queryByText("video")).not.toBeInTheDocument();
		expect(queryByText("image")).not.toBeInTheDocument();
		expect(queryByText("file")).not.toBeInTheDocument();

		// we have the image editor automatically loaded
		const editor = container.querySelector("div.editor-image");
		expect(editor).toBeTruthy();
	});

	it("if file url is passed in, should display the file editor and not the button group", () => {
		const { container, queryByText } = renderComponent({
			value: "https://somethingsomething.csv",
		});

		expect(queryByText("video")).not.toBeInTheDocument();
		expect(queryByText("image")).not.toBeInTheDocument();
		expect(queryByText("file")).not.toBeInTheDocument();

		// we have the file editor automatically loaded
		const editor = container.querySelector("div.editor-file");
		expect(editor).toBeTruthy();
	});

	it("video is not in acceptedTypes and the value is a video url, video editor is not displayed", () => {
		const { container } = renderComponent({
			value: "https://players.brightcove.com/455454",
			acceptTypes: ["image", "file"],
		});

		const editor = container.querySelector("div.editor-video");
		expect(editor).toBeFalsy();
	});

	it("image is not in acceptedTypes and the value is an image url, image editor is not displayed", () => {
		const { container } = renderComponent({
			value: "https://something.svg",
			acceptTypes: ["video", "file"],
		});

		const editor = container.querySelector("div.editor-image");
		expect(editor).toBeFalsy();
	});

	it("displays a hint", () => {
		const { getByText } = renderComponent({
			hint: "it has three ears",
		});

		getByText("it has three ears");
	});
});
