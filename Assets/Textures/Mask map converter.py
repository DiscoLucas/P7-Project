import os
from PIL import Image

# Define paths
root_folder = './'
mask_folder = './Mask map/'

# Create Mask map folder if it doesn't exist
if not os.path.exists(mask_folder):
    os.makedirs(mask_folder)

# Function to get the correct texture file based on name pattern
def get_texture_file(name_part, file_list):
    for file_name in file_list:
        if name_part in file_name:
            return file_name
    return None

# Function to create a black (null) image if the texture is missing
def create_null_image(size):
    return Image.new('L', size, 0)  # 'L' mode for grayscale, 0 for black

# Process each set of textures
for root, dirs, files in os.walk(root_folder):
    for file in files:
        if file.endswith(('.jpg', '.png')) and 'Color' in file:
            # Extract base name (e.g., Concrete042B_4K)
            base_name = file.split('_Color')[0]

            # Check if the mask map already exists
            output_path = os.path.join(mask_folder, f"{base_name}_MaskMap.png")
            if os.path.exists(output_path):
                print(f"Mask map already exists for {base_name}, skipping...")
                continue

            # Get corresponding textures
            metal_file = get_texture_file('_Metalness', files)
            ao_file = get_texture_file('_AmbientOcclusion', files)
            detail_file = get_texture_file('_Detail', files)
            smoothness_file = get_texture_file('_Roughness', files)

            # Open images as grayscale, or create null image if missing
            if metal_file:
                metal_img = Image.open(os.path.join(root_folder, metal_file)).convert('L')
            else:
                metal_img = create_null_image(metal_img.size)
                print(f"Missing Metalness texture for {base_name}, using black.")

            if ao_file:
                ao_img = Image.open(os.path.join(root_folder, ao_file)).convert('L')
            else:
                ao_img = create_null_image(metal_img.size)
                print(f"Missing AO texture for {base_name}, using black.")

            if detail_file:
                detail_img = Image.open(os.path.join(root_folder, detail_file)).convert('L')
            else:
                detail_img = create_null_image(metal_img.size)
                print(f"Missing Detail texture for {base_name}, using black.")

            if smoothness_file:
                smoothness_img = Image.open(os.path.join(root_folder, smoothness_file)).convert('L')
            else:
                smoothness_img = create_null_image(metal_img.size)
                print(f"Missing Smoothness texture for {base_name}, using black.")

            # Create an empty RGBA image
            width, height = metal_img.size
            mask_map = Image.new('RGBA', (width, height))

            # Combine channels into the mask map
            mask_map = Image.merge('RGBA', (metal_img, ao_img, detail_img, smoothness_img))

            # Save the mask map
            mask_map.save(output_path)
            print(f"Processed and saved: {output_path}")

print("All done!")