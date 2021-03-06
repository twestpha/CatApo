# Deletes all material folders and *.blend1 files autogenerated by blender in all subdirectories

import sys
import os
import shutil
import ctypes

def OpenAndDeleteRecursive(fpath):
    for root, subfolders, files in os.walk(fpath):
        if("Materials") in subfolders:
            path = os.path.join(root, "Materials")

        for fname in files:
            if("blend1" in fname):
                os.remove(os.path.join(root, fname))
            if("Materials" in fname):
                os.remove(os.path.join(root, fname))

OpenAndDeleteRecursive(".");
