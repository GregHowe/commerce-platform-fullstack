import { get as _get, clone as _clone } from "lodash";
// it is possible this will generate an id that is not unique
// very unlikely but still.. there is a function in the site store
// that tracks used ids and makes sure generated ids are unique
function _generateRandomId(length = 6) {
	const letters = "bcdfghjkmnpqrstvwxyz";
	const characters = letters + "23456789";
	let id = letters[Math.floor(Math.random() * letters.length)];
	for (let i = 1; i < length; i++) {
		id += characters[Math.floor(Math.random() * characters.length)];
	}
	return id;
}

function _getBlank() {
	return {
		id: null,
		type: null,
		variants: {},
	};
}

// recursively searches for a block id
// returning something like [0,2,1,0] representing a list of block indexes
// from shallow to deep where the target block is nested
function _findIndexDeep(blocks, blockId, indexChain = []) {
	if (!blocks) {
		// eslint-disable-next-line no-console
		console.error("Find index requires blocks");
		return -1;
	}
	if (!blockId) {
		return -1;
	}
	let found = blocks.findIndex((b) => b.id === blockId);
	if (found !== -1) {
		return [...indexChain, found];
	}
	blocks.forEach((block, blockIndex) => {
		if (block?.blocks) {
			const deepIndexes = _findIndexDeep(block.blocks, blockId);
			if (deepIndexes !== -1) {
				found = [...indexChain, blockIndex, ...deepIndexes];
				return;
			}
		}
	});
	if (found !== -1) {
		return [...indexChain, ...found];
	}
	return found;
}

export default {
	generateId() {
		return _generateRandomId();
	},

	getBlank() {
		return _getBlank();
	},

	find(blocks, id) {
		const blockIndexes = _findIndexDeep(blocks, id);
		if (blockIndexes === -1) {
			return null;
		}
		const path = "[" + blockIndexes.join("].blocks[") + "]";
		const found = _get(blocks, path, null);
		return found;
	},

	findIndexes(blocks, id) {
		return _findIndexDeep(blocks, id) || null;
	},

	insert(blocks, position) {
		const positionClone = _clone(position);
		const deepestIndex = positionClone.pop();
		let deepestBlocks = null;
		if (positionClone.length) {
			const deepestBlocksPath =
				"[" + positionClone.join("].blocks[") + "].blocks";
			deepestBlocks = _get(blocks, deepestBlocksPath, []);
		} else {
			deepestBlocks = blocks;
		}
		const newBlock = _getBlank();
		if (deepestIndex === 0) {
			deepestBlocks.unshift(newBlock);
		} else if (deepestIndex >= blocks.length) {
			deepestBlocks.push(newBlock);
		} else {
			deepestBlocks.splice(deepestIndex, 0, newBlock);
		}
		return {
			blocks,
			newBlock,
		};
	},

	move(blocks, id, direction) {
		const blockIndexes = _findIndexDeep(blocks, id);
		if (blockIndexes) {
			const deepestBlockIndex = blockIndexes.pop();
			let deepestBlocks = null;
			if (blockIndexes.length) {
				const deepestBlocksPath =
					"[" + blockIndexes.join("].blocks[") + "].blocks";
				deepestBlocks = _get(blocks, deepestBlocksPath, []);
			} else {
				deepestBlocks = blocks;
			}
			let targetBlock = null;
			if (direction === "up" || direction === "left") {
				if (deepestBlockIndex > 0) {
					targetBlock = deepestBlocks.splice(deepestBlockIndex, 1)[0];
					deepestBlocks.splice(deepestBlockIndex - 1, 0, targetBlock);
				}
			} else if (direction === "down" || direction === "right") {
				if (deepestBlockIndex < deepestBlocks.length - 1) {
					targetBlock = deepestBlocks.splice(deepestBlockIndex, 1)[0];
					deepestBlocks.splice(deepestBlockIndex + 1, 0, targetBlock);
				}
			}
		}
		return blocks;
	},

	remove(blocks, id) {
		const blockIndexes = _findIndexDeep(blocks, id);
		if (blockIndexes) {
			const deepestBlockIndex = blockIndexes.pop();
			let deepestBlocks = null;
			if (blockIndexes.length) {
				const deepestBlocksPath =
					"[" + blockIndexes.join("].blocks[") + "].blocks";
				deepestBlocks = _get(blocks, deepestBlocksPath, []);
			} else {
				deepestBlocks = blocks;
			}
			deepestBlocks.splice(deepestBlockIndex, 1);
		}
		return blocks;
	},
};
